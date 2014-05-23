using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Usergroup.Sugnl.JsonReadSpike;
using Website.JsonRead;

namespace Website.Controllers
{
    public class WikiController : Controller
    {
        public class ParseHandler : IHandler<Instance>
        {
            private readonly Database db = Sitecore.Configuration.Factory.GetDatabase("master");
            private readonly Item placesFolder = Sitecore.Configuration.Factory.GetDatabase("master").GetItem("/sitecore/content/global/places");
            private readonly Item categoriesFolder = Sitecore.Configuration.Factory.GetDatabase("master").GetItem("/sitecore/content/global/categories");
            private readonly TemplateID placeTemplateId = new TemplateID(new ID("{F5A7F393-04E6-49E1-8D16-C5A3229DB8BC}"));
            private readonly TemplateID categoryTemplateId = new TemplateID(new ID("{780855E4-FDCC-4D32-9606-AF70BEB88F42}"));
            private readonly Dictionary<string, string> categories = new Dictionary<string, string>();

            private int counter = 0;

            public void Process(Instance item)
            {
                counter++;
                if (counter % 1000 == 0) Sitecore.Diagnostics.Log.Info("---> Created: " + counter, this);
                var place = placesFolder.Add(ItemUtil.ProposeValidItemName(item.Label), placeTemplateId);
                place.Editing.BeginEdit();

                var categoriesField = new MultilistField(place.Fields["categories"]);
                foreach (var category in item.Categories)
                {
                    categoriesField.Add(GetCategory(category));
                }
                place["id"] = item.ID;
                place["label"] = item.Label;
                place["description"] = item.Description;
                place["point"] = (item.Point != "" && item.Point != "NULL") ? item.Point : "";

                place.Editing.EndEdit();
            }

            private string GetCategory(string name)
            {
                if (!categories.ContainsKey(name))
                {
                    Sitecore.Diagnostics.Log.Info("---> Starting", this);

                    var cat = db.GetItem("/sitecore/content/global/categories/" + ItemUtil.ProposeValidItemName(name));
                    if (cat == null)
                    {
                        cat = categoriesFolder.Add(ItemUtil.ProposeValidItemName(name), categoryTemplateId);
                        categories.Add(name, cat.ID.ToString());
                    }
                    else
                    {
                        categories.Add(name, cat.ID.ToString());
                    }
                }
                return categories[name];
            }


        }

        //
        // GET: /Wiki/
        public ActionResult Run()
        {
            Sitecore.Configuration.Settings.Indexing.Enabled = false;
            using (new SecurityDisabler())
            {
                using (new EventDisabler())
                {
                    var fileStream = new FileStream(@"<absolute-path-to>\PopulatedPlace.json", FileMode.Open);
                    var parser = new InstanceParser(fileStream, "http://dbpedia.org/resource");
                    parser.Parse(new ParseHandler());
                }
            }
            Sitecore.Configuration.Settings.Indexing.Enabled = true;

            return View();

        }
	}
}