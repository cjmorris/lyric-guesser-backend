var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Added CORS
builder.Services.AddCors(options => {
    options.AddPolicy("FrontEnd", policyBuilder => {
        policyBuilder.WithOrigins("http://localhost:5173");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/lyrics", () =>
{
    string[] lyrics =  ["Buddy","you're","a","boy","make","a","big","noise",
        "Playing","in","the","street","gonna","be","a","big","man","someday",
        "You","got","mud","on","your","face","you","big","disgrace",
        "Kicking","your","can","all","over","the","place","singin'",
        "We","will","we","will","rock","you",
        "We","will","we","will","rock","you",
        "Buddy","you're","a","young","man","hard","man",
        "Shouting","in","the","street","gonna","take","on","the","world","someday",
        "You","got","blood","on","your","face","you","big","disgrace",
        "Waving","your","banner","all","over","the","place",
        "We","will","we","will","rock","you","sing","it",
        "We","will","we","will","rock","you",
        "Buddy","you're","an","old","man","poor","man",
        "Pleading","with","your","eyes","gonna","make","you","some","peace","someday",
        "You","got","mud","on","your","face","big","disgrace",
        "Somebody","better","put","you","back","into","your","place",
        "We","will","we","will","rock","you","sing","it",
        "We","will","we","will","rock","you","everybody",
        "We","will","we","will","rock","you","hmm",
        "We","will","we","will","rock","you",
        "Alright"];
    return lyrics;
})
.WithName("Lyrics")
.WithOpenApi();

//Enabling CORS
app.UseCors("FrontEnd");

app.Run();