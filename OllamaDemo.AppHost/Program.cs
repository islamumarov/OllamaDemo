var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.OllamaDemo_ApiService>("apiservice");

builder.AddProject<Projects.OllamaDemo_Web>("webfrontend")
    .WithReference(apiservice);

builder.Build().Run();
