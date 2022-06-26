/************************************************************************************************************
 * Project_name: EducationSystem                                                                            *
 * Description:  Website for facilitating the work of schools,                                              *
 *               educational institutions throughout Russia                                                 *
 * Backendman:   Nikita Lomeiko    | https://vk.com/id390878963                                             *
 * Frontendman:  Eugeniy Spiglazov | https://vk.com/wtf_yours_id                                            *
 ************************************************************************************************************/

var web = new EducationSystem.Web.Api.Buid.WebBuilder(args); // custom class for build server

// -----------------------------------= Add services =---------------------------------------------- //

web.AddDb();                                // add context for work with db
web.AddAuthorizeAuthentication();           // add authorize and authentication with settings jwt
web.AddCors();                              // add cord for client server application
web.AddControllers();                       // add controllers
web.AddServicesControllers();               // add service for controllers
web.AddServicesCrud();                      // add service 'Crud'
web.AddModelOptions();                      // add option models
web.AddServiceHelp();                       // add help service
web.AddServiceHttpContextAccessor();

// ------------------------------= Set middlewars * Start server =---------------------------------- //

web.AppBuild();
web.App.Run();