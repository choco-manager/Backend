using Backend.Data;

using Serilog;


namespace Backend.Middlewares; 

public class CheckPendingDatabaseChangesMiddleware: IMiddleware {

  private readonly ApplicationDbContext _db;
  public CheckPendingDatabaseChangesMiddleware(ApplicationDbContext db) {
    _db = db;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
    await next(context);

    if (_db.ChangeTracker.HasChanges())
    {
      Log.Warning("Has unapplied database changes");
    }
  }
}