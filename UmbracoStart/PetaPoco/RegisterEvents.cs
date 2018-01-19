using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using UmbracoStart.PetaPoco.Entity;

namespace UmbracoStart.PetaPoco
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //Get the Umbraco Database context
            var db = applicationContext.DatabaseContext.Database;

            //Check if the DB table does NOT exist
            if (!db.TableExist("PetaPocoUser"))
            {
                //Create DB table - and set overwrite to false
                db.CreateTable<PPUserEntity>(false);
            }

            //Example of other events (such as before publish)
            Document.BeforePublish += Document_BeforePublish;
        }

        //Example Before Publish Event
        private void Document_BeforePublish(Document sender, PublishEventArgs e)
        {
            //Do what you need to do. In this case logging to the Umbraco log
            Log.Add(LogTypes.Debug, sender.Id, "the document " + sender.Text + " is about to be published");

            //cancel the publishing if you want.
            e.Cancel = true;
        }
    }
}