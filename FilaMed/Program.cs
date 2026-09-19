using FilaMed.Service;

var p1  = TicketService.GenerateTicket();
var p2 = TicketService.GenerateTicket();
var p3 = TicketService.GenerateTicket();
var p4 = TicketService.GenerateTicket();

Console.WriteLine($"Paciente 1: {p1}");
Console.WriteLine($"Paciente 2: {p2}");
Console.WriteLine($"Paciente 3: {p3}");
Console.WriteLine($"Paciente 4: {p4}");




var queue = new QueueService();


queue.AddTicket(p1);
queue.AddTicket(p2);
queue.AddTicket(p3);
queue.AddTicket(p4);






