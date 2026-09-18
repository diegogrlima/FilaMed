using FilaMed.Service;

var p1  = TicketService.GeneratePassword();
var p2 = TicketService.GeneratePassword();
var p3 = TicketService.GeneratePassword();
var p4 = TicketService.GeneratePassword();
var p5 = TicketService.GeneratePassword();

Console.WriteLine($"Paciente 1: {p1}");
Console.WriteLine($"Paciente 2: {p2}");
Console.WriteLine($"Paciente 3: {p3}");
Console.WriteLine($"Paciente 4: {p4}");
Console.WriteLine($"Paciente 5: {p5}");