using Microsoft.EntityFrameworkCore;

namespace Fitness.Notification;

public class NotificationBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await CheckForNotifications(context);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private static async Task CheckForNotifications(AppDbContext context)
    {
        try
        {
            var users = await context.Users
                .Include(u => u.WaterDiets)
                .ToListAsync();

            foreach (var user in users)
            {
                var waterDiets = user.WaterDiets.Where(wd => wd.CreationDate.Day == DateTime.UtcNow.Day &&
                                                             wd.CreationDate.Month == DateTime.UtcNow.Month &&
                                                             wd.CreationDate.Year == DateTime.UtcNow.Year);
                var normal = user.Weight * 35;
                var total = waterDiets.Sum(water => water.Count);

                if (total >= normal) continue;
                Console.WriteLine($"Send ${user.Username} ${normal} ${total}");
                await SendNotificationToUser(context, user, normal, total);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static async Task SendNotificationToUser(AppDbContext context, User.User user, decimal normal, decimal total)
    {
        var usersNotifications = await context.Notifications
            .Include(n => n.User)
            .Where(n => n.User.Id.Equals(user.Id))
            .ToListAsync();

        var text = $"Вам осталось сегодня выпить {Math.Floor(normal - total)} мл";

        if (!usersNotifications.Any(un => un.Text.Equals(text)))
        {
            var notification = new Notification
            {
                User = user,
                Text = text,
                IsRead = false
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
            return;
        }

        var userNotification = usersNotifications.FirstOrDefault(un => un.Text.Equals(text));

        if (userNotification != null)
        {
            userNotification.Text = text;
            userNotification.IsRead = false;
            userNotification.CreationDate = DateTime.UtcNow;

            await context.SaveChangesAsync();   
        }
    }
}