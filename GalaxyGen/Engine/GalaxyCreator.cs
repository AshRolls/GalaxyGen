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
            gal.MaxId = 100;

            SolarSystem ss = this.GetSolarSystem("Sol");

            Agent ag = this.GetAgent("The Mule");
            Producer prod = this.GetProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            prod.Owner = ag;
            ag.Producers.Add(prod);
            Producer prod2 = this.GetProducer("Factory Spice", BluePrintEnum.PlatinumToSpice);
            prod2.Owner = ag;
            ag.Producers.Add(prod2);

            ss.Agents.Add(ag);

            Planet p = this.GetPlanet("Earth", 150000000d, 365);
            addNewStoreToPlanet(p, ag);
            p.Producers.Add(prod);
            p.Producers.Add(prod2);
            ss.Planets.Add(p);

            Planet p2 = this.GetPlanet("Mars", 227000000d, 687);
            ss.Planets.Add(p2);

            Ship s = this.GetShip("Whitestar");
            s.Owner = ag;
            s.ShipState = ShipStateEnum.Docked;
            s.Agents.Add(ag);
            ag.Location = s;
            s.Pilot = ag;
            ag.AgentState = AgentStateEnum.PilotingShip;
            s.DockedPlanet = p;
            p.DockedShips.Add(s);
            ag.ShipsOwned.Add(s);
            addNewCargoStoreToShip(s, ag);
            s.SolarSystem = ss;
            ss.Ships.Add(s);

            gal.SolarSystems.Add(ss);

            //for (int i = 0; i < 5000; i++)
            //{
            //    SolarSystem ss1 = this.GetSolarSystem(i.ToString());
            //    Planet p1 = this.GetPlanet(i.ToString(), 150000000d, 365);
            //    Agent newAg = this.GetAgent(i.ToString());
            //    newAg.Location = p1;
            //    Producer pr = this.GetProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            //    pr.Owner = newAg;
            //    newAg.Producers.Add(pr);
            //    p1.Producers.Add(pr);
            //    Producer pr2 = this.GetProducer("Factory Spice", BluePrintEnum.PlatinumToSpice);
            //    pr2.Owner = newAg;
            //    newAg.Producers.Add(pr2);
            //    p1.Producers.Add(pr2);
            //    addNewStoreToPlanet(p1, newAg);
            //    ss1.Agents.Add(newAg);
            //    ss1.Planets.Add(p1);
            //    gal.SolarSystems.Add(ss1);
            //}

            //for (int i = 5001; i < 10000; i++)
            //{
            //    SolarSystem ss1 = this.GetSolarSystem(i.ToString());
            //    Planet p1 = this.GetPlanet(i.ToString(), 150000000d, 365);
            //    Agent newAg = this.GetAgent(i.ToString());
            //    newAg.Location = p1;
            //    Producer pr = this.GetProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            //    pr.Owner = newAg;
            //    newAg.Producers.Add(pr);
            //    p1.Producers.Add(pr);
            //    Producer pr2 = this.GetProducer("Factory Spice", BluePrintEnum.PlatinumToSpice);
            //    pr2.Owner = newAg;
            //    newAg.Producers.Add(pr2);
            //    p1.Producers.Add(pr2);
            //    addNewStoreToPlanet(p1, newAg);
            //    ss1.Agents.Add(newAg);
            //    ss1.Planets.Add(p1);
            //    gal.SolarSystems.Add(ss1);
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

        public Planet GetPlanet(string seedName, Double orbitalKm, Double orbitalDays)
        {
            Planet plan = new Planet();
            plan.Population = 10000;
            plan.OrbitKm = orbitalKm;
            plan.OrbitDays = orbitalDays;
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
            p.Stores.Add(o.AgentId,s);
            o.Stores.Add(s);

             // seed with basic starter resource
            s.StoredResources.Add(ResourceTypeEnum.Spice, 100);
            s.StoredResources.Add(ResourceTypeEnum.Platinum, 100);            
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
            prod.ProduceNThenStop = 0;
            return prod;
        }

        private void addNewCargoStoreToShip(Ship ship, Agent o)
        {
            Store s = new Store();
            s.Owner = o;
            s.Location = ship;
            ship.Stores.Add(o.AgentId,s);           
            o.Stores.Add(s);

            // seed with basic starter resource
            s.StoredResources.Add(ResourceTypeEnum.Spice, 10);
        }

        public Ship GetShip(String seedName)
        {
            Ship s = new Ship();
            s.Name = seedName;                        
            return s;
        }

        //public ResourceQuantity GetResourceQuantity(ResourceTypeEnum type, Int64 qty)
        //{
        //    ResourceQuantity resQ = new ResourceQuantity();
        //    resQ.Type = type;
        //    resQ.Quantity = qty;
        //    return resQ;
        //}     

    }
}
