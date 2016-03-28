using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGen.Model;
using Ninject;
using GalaxyGen.Framework;

namespace GalaxyGen.Engine
{
    public class GalaxyCreator : IGalaxyCreator
    {
        public Galaxy GetFullGalaxy()
        {
            Galaxy gal = this.GetGalaxy();

            SolarSystem ss = this.GetSolarSystem("Sol");

            Agent ag = this.GetAgent("The Mule");
            Producer prod = this.GetProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            prod.Owner = ag;
            ag.Producers.Add(prod);
            Producer prod2 = this.GetProducer("Factory Spice", BluePrintEnum.PlatinumToSpice);            
            prod2.Owner = ag;
            ag.Producers.Add(prod2);

            ss.Agents.Add(ag);           

            Planet p = this.GetPlanet("Earth");
            addNewStoreToPlanet(p, ag);
            p.Producers.Add(prod);            
            p.Producers.Add(prod2);
            ss.Planets.Add(p);

            Planet p2 = this.GetPlanet("Mars");
            
            ss.Planets.Add(p2);
            gal.SolarSystems.Add(ss);

            //for (int i = 0; i < 5000; i++)
            //{
            //    Agent newAg = this.GetAgent(i.ToString());
            //    Producer pr = this.GetProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            //    pr.Owner = newAg;
            //    newAg.Producers.Add(pr);
            //    p.Producers.Add(pr);
            //    Producer pr2 = this.GetProducer("Factory Spice", BluePrintEnum.PlatinumToSpice);
            //    pr2.Owner = newAg;
            //    newAg.Producers.Add(pr2);
            //    p.Producers.Add(pr2);
            //    addNewStoreToPlanet(p, newAg);
            //    ss.Agents.Add(newAg);
            //}

            //for (int i = 5001; i < 10000; i++)
            //{
            //    Agent newAg = this.GetAgent(i.ToString());
            //    Producer pr = this.GetProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            //    pr.Owner = newAg;
            //    newAg.Producers.Add(pr);
            //    p2.Producers.Add(pr);
            //    Producer pr2 = this.GetProducer("Factory Spice", BluePrintEnum.PlatinumToSpice);
            //    pr2.Owner = newAg;
            //    newAg.Producers.Add(pr2);
            //    p2.Producers.Add(pr2);
            //    addNewStoreToPlanet(p2, newAg);
            //    ss.Agents.Add(newAg);
            //}

            //for (int i = 0; i < 500; i++)
            //{
            //    SolarSystem ss2 = this.GetSolarSystem(i.ToString());
            //    ss2.Planets.Add(this.GetPlanet("Earth"));
            //    ss2.Planets.Add(this.GetPlanet("Mars"));
            //    gal.SolarSystems.Add(ss2);
            //}

            return gal;
        }

        public Galaxy GetGalaxy()
        {
            Galaxy gal = new Galaxy();
            gal.Name = "Milky Way";            
            return gal;
        }

        public SolarSystem GetSolarSystem(string seedName)
        {
            SolarSystem sys = new SolarSystem();
            sys.Name = seedName;
            return sys;
        }

        public Planet GetPlanet(string seedName)
        {
            Planet plan = new Planet();
            plan.Population = 10000;
            plan.Name = seedName;
            Society soc = new Society();
            soc.Name = seedName + " Soc";
            plan.Society = soc;           

            //IMarket mar = kernel.Get<IMarket>();            
            
            //IMarketBuyOrder mbo = kernel.Get<IMarketBuyOrder>();
            //IAgent agent = kernel.Get<IAgent>();
            //agent.Name = "Agent 1";
            //mbo.Owner = agent;
            //mbo.Quantity = 5;
            //mbo.Type = ResourceTypeEnum.Spice;
            //mar.BuyOrders.Add(mbo);

            //plan.Market = mar;

            return plan;
        }

        private void addNewStoreToPlanet(Planet p, Agent o)
        {
            Store s = new Store();
            s.Owner = o;
            s.Location = p;            
            p.Stores.Add(s);
            o.Stores.Add(s);

            ResourceQuantity resQ = new ResourceQuantity(); // seed with basic starter resource
            resQ.Type = ResourceTypeEnum.Spice;
            resQ.Quantity = 100;
            s.StoredResources.Add(resQ);

            resQ = new ResourceQuantity(); // seed with basic starter resource
            resQ.Type = ResourceTypeEnum.Platinum;
            resQ.Quantity = 100;
            s.StoredResources.Add(resQ);
        }

        public Agent GetAgent(String seedName)
        {
            Agent ag = new Agent();
            ag.Name = seedName;
            return ag;
        }

        public Producer GetProducer(String seedName, BluePrintEnum bpType)
        {
            Producer prod = new Producer();
            prod.Name = seedName;
            prod.BluePrintType = bpType;
            prod.Producing = false;
            prod.AutoResumeProduction = true;        
            return prod;
        }       

    }
}
