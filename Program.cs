using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MidDishApp.Data;
using MidDishApp.Services;

// Fixed culture so decimals ("12.50") parse the same on every machine.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    // Framework binding errors -> translation keys (translated in the views by Html.Err)
    var p = options.ModelBindingMessageProvider;
    p.SetValueMustBeANumberAccessor(_ => "Val.Number");
    p.SetAttemptedValueIsInvalidAccessor((_, __) => "Val.Invalid");
    p.SetUnknownValueIsInvalidAccessor(_ => "Val.Invalid");
    p.SetValueIsInvalidAccessor(_ => "Val.Invalid");
    p.SetValueMustNotBeNullAccessor(_ => "Val.Required");
    p.SetMissingBindRequiredValueAccessor(_ => "Val.Required");
    p.SetMissingKeyOrValueAccessor(() => "Val.Required");
    p.SetNonPropertyAttemptedValueIsInvalidAccessor(_ => "Val.Invalid");
    p.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Val.Invalid");
    p.SetNonPropertyValueMustBeANumberAccessor(() => "Val.Number");
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILocalizer, Localizer>();
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Create the database (name from appsettings, e.g. BIT240009_MID) + seed sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    SeedData.Init(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStatusCodePagesWithReExecute("/Home/Status", "?code={0}");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dishes}/{action=Index}/{id?}");

app.Run();
