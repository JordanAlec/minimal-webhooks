using Api.Models;

namespace Api.Data;

public static class InMemoryAlerts
{
    public static Dictionary<int, Alert> Alerts = new Dictionary<int, Alert>
    {
        {1, new Alert{ Id = 1, Title = "Alert Example 1", Status = "Resolved"}},
        {2, new Alert{ Id = 2, Title = "Alert Example 2", Status = "Resolved"}},
        {3, new Alert{ Id = 3, Title = "Alert Example 3", Status = "In Progress"}},
    };
}