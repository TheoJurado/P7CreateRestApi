using P7CreateRestApi.Data;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Data;

namespace P7CreateRestApi
{
    public static class DbInitializer
    {
        public static async Task<IApplicationBuilder> SeedDatabaseAsync(this IApplicationBuilder app)
        {
            // Ensure that the 'app' parameter is not null. Throws an exception if it is.
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            // Create a new service scope to access scoped services (e.g., DbContext).
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            // Get the logger service to log messages and errors.
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                // Log the beginning of the database migration process.
                logger.LogInformation("Attempting to migrate database...");

                // Retrieve the application's database context.
                var context = services.GetRequiredService<LocalDbContext>();

                // Check if the context is null and log an error if so, halting further execution.
                if (context == null)
                {
                    logger.LogError("ApplicationDbContext is not available. Seeding cannot proceed.");
                    return app;
                }

                // Apply any pending migrations to the database to ensure it is up to date.
                context.Database.Migrate();

                // Log the completion of the database migration.
                logger.LogInformation("Database migration completed. Starting database seeding...");

                // Initialize and seed the database with default data.
                await SeedData.Initialize(services);

                // Log the successful completion of database seeding.
                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                // Log an error if there is a database update-related issue.
                logger.LogError($"Error during database update: {dbEx.Message}");

                // Log additional details if there is an inner exception.
                if (dbEx.InnerException != null)
                {
                    logger.LogError($"Additional details: {dbEx.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors during the database seeding process.
                logger.LogError(ex, "An unexpected error occurred during database seeding.");
            }

            // Return the application builder instance to continue application setup.
            return app;
        }
    }
}
