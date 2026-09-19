using FilaMed.Service;

namespace FilaMed.Ui
{
    internal class Menu
    {
        private static readonly string[] Options =
        [
            "Gerar nova senha",
            "Visualizar fila",
            "Chamar próximo paciente",
            "Quantidade na fila",
            "Sair"
        ];

        private readonly QueueService _queue = new();
        private readonly bool _interactive = !Console.IsInputRedirected && !Console.IsOutputRedirected;
        private int _selectedIndex;

        public void Run()
        {
            bool cursorVisible = true;
            if (_interactive)
            {
                if (OperatingSystem.IsWindows()) cursorVisible = Console.CursorVisible;
                Console.CursorVisible = false;
            }

            try
            {
                while (true)
                {
                    DrawHeader("MENU PRINCIPAL");
                    int optionsTop = _interactive ? Console.CursorTop : 0;
                    DrawOptions();
                    Console.WriteLine();
                    Console.WriteLine(_interactive
                        ? "  ↑ ↓ Navegar | Enter Selecionar | Esc Sair"
                        : "  Digite uma opção de 0 a 4:");

                    int selected = ReadSelection(optionsTop);
                    if (selected == 4)
                    {
                        if (_interactive) Console.Clear();
                        Console.WriteLine("Até logo! FilaMed encerrado.");
                        return;
                    }

                    ExecuteOption(selected);
                }
            }
            finally
            {
                Console.ResetColor();
                if (_interactive) Console.CursorVisible = cursorVisible;
            }
        }

        private void DrawHeader(string title)
        {
            if (_interactive)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
            }

            Console.WriteLine("  ════════════════════════════════════════");
            Console.WriteLine("                  FILAMED");
            Console.WriteLine("        Atendimento de pacientes");
            Console.WriteLine("  ════════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine($"  Pacientes aguardando: {_queue.CountTickets()}");
            Console.WriteLine();
            Console.WriteLine($"  {title}");
            Console.WriteLine();
        }

        private void DrawOptions()
        {
            for (int i = 0; i < Options.Length; i++)
            {
                bool selected = i == _selectedIndex;
                if (_interactive && selected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine($"  {(selected ? "›" : " ")} [{i}] {Options[i],-25} ");
                Console.ResetColor();
            }
        }

        private int ReadSelection(int optionsTop)
        {
            while (true)
            {
                if (!_interactive)
                {
                    string? input = Console.ReadLine();
                    if (input is null) return 4;
                    if (int.TryParse(input, out int option) && option >= 0 && option < Options.Length)
                    {
                        _selectedIndex = option;
                        return option;
                    }

                    Console.WriteLine("  Opção inválida. Escolha uma opção de 0 a 4.");
                    continue;
                }

                ConsoleKey key = Console.ReadKey(intercept: true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        _selectedIndex = (_selectedIndex + Options.Length - 1) % Options.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        _selectedIndex = (_selectedIndex + 1) % Options.Length;
                        break;
                    case ConsoleKey.Enter:
                        return _selectedIndex;
                    case ConsoleKey.Escape:
                        return 4;
                    default:
                        continue;
                }

                Console.SetCursorPosition(0, optionsTop);
                DrawOptions();
            }
        }

        private void ExecuteOption(int option)
        {
            switch (option)
            {
                case 0:
                    var ticket = TicketService.GenerateTicket();
                    _queue.AddTicket(ticket);
                    ShowResult("SENHA GERADA", $"Senha gerada: {ticket}");
                    break;
                case 1:
                    ShowQueue();
                    break;
                case 2:
                    string message = _queue.CountTickets() == 0
                        ? "A fila está vazia. Nenhum paciente aguardando."
                        : $"Próximo paciente: {_queue.CallNextTicket()}";
                    ShowResult("CHAMAR PACIENTE", message);
                    break;
                case 3:
                    ShowResult("QUANTIDADE NA FILA", $"Quantidade na fila: {_queue.CountTickets()}");
                    break;
            }
        }

        private void ShowResult(string title, string message)
        {
            DrawHeader(title);
            Console.WriteLine($"  {message}");
            WaitForReturn();
        }

        private void WaitForReturn()
        {
            if (!_interactive) return;
            Console.WriteLine();
            Console.WriteLine("  Pressione qualquer tecla para voltar.");
            Console.ReadKey(intercept: true);
        }

        private void ShowQueue()
        {
            var tickets = _queue.GetAllTickets();
            if (tickets.Count == 0)
            {
                ShowResult("FILA DE ATENDIMENTO", "A fila está vazia.");
                return;
            }

            int orderWidth = Math.Max("Ordem".Length, tickets.Count.ToString().Length);
            int ticketWidth = Math.Max("Senha".Length, tickets.Max(ticket => ticket.Name.Length));
            string header = $"  {"Ordem".PadRight(orderWidth)} | {"Senha".PadRight(ticketWidth)} | Emissão";
            string separator = $"  {new string('-', orderWidth)}-+-{new string('-', ticketWidth)}-+--------";

            int page = 0;
            while (true)
            {
                int pageSize = _interactive ? Math.Max(1, Console.WindowHeight - 15) : tickets.Count;
                int pageCount = (tickets.Count + pageSize - 1) / pageSize;
                page = Math.Min(page, pageCount - 1);
                DrawHeader("FILA DE ATENDIMENTO");
                Console.WriteLine(header);
                Console.WriteLine(separator);
                for (int i = page * pageSize; i < Math.Min((page + 1) * pageSize, tickets.Count); i++)
                {
                    var ticket = tickets[i];
                    Console.WriteLine($"  {(i + 1).ToString().PadRight(orderWidth)} | {ticket.Name.PadRight(ticketWidth)} | {ticket.IssuedAt:HH:mm}");
                }

                Console.WriteLine();
                Console.WriteLine($"  Página {page + 1} de {pageCount}");
                if (!_interactive) return;
                Console.WriteLine("  ← → Páginas | Enter / Esc Voltar");

                switch (Console.ReadKey(intercept: true).Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.PageUp:
                        page = Math.Max(0, page - 1);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.PageDown:
                        page = Math.Min(pageCount - 1, page + 1);
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
