using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQL.Server.Ui.Voyager;
using CommanderGQL.GraphQL.Platforms;
using CommanderGQL.GraphQL.Commands;

namespace CommanderGQL
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<AppDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("CommandConStr")));

            services
            .AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<Subscription>()
            .AddType<PlatformType>()
            .AddType<CommandType>()
            .AddSorting()
            .AddFiltering()
            .AddInMemorySubscriptions();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

            app.UseGraphQLVoyager(new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql"
            }, "/graphql-voyager");
        }
    }
}
