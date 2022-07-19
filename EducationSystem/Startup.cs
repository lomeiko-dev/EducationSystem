var web = new EducationSystem.Web.Api.Buid.WebBuilder(args); // custom class for build server

// -----------------------------------= Add services =---------------------------------------------- //

web.AddDb();                                // add context for work with db
web.AddCors();                              // add cord for client server application
web.AddControllers();                       // add controllers
web.AddServiceHttpContextAccessor();
web.AddServicesCrud();                      // add service 'Crud'
web.AddModelOptions();                      // add option models
web.AddServiceHelp();                       // add help service
web.AddAuthorizeAuthentication();           // add authorize and authentication with settings jwt
web.AddServicesControllers();               // add service for controllers


// ------------------------------= Set middlewars * Start server =---------------------------------- //

web.AppBuild();
