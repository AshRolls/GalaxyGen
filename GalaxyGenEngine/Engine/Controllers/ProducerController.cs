using Akka.Actor;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Model;
using GalaxyGenCore.BluePrints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGenCore.Resources;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class ProducerController
    {
        private Producer _model;
        private BluePrint _bp;
        private SolarSystemController _solarSystemC;
        private PlanetController _planetC;
        private TextOutputController _textOutput;        

        public ProducerController(Producer p, SolarSystemController ssc, PlanetController pc, TextOutputController textOutput)
        {
            _model = p;
            _solarSystemC = ssc;
            _planetC = pc;
            _textOutput = textOutput;
            _bp = BluePrints.GetBluePrint(p.BluePrintType);            
        }

        public void Tick(MessageTick tick)
        {
            if (_model.Producing)
            {
                producingTick(tick);
            }
            else if (_model.AutoResumeProduction || _model.ProduceNThenStop > 0)
            {                
                tryStartProduction(tick, false);
            }
        }

        private void producingTick(MessageTick tick)
        {
            if (_model.TickForNextProduction <= tick.Tick)
            {                      
                _planetC.AddResources(_bp.Produces, _model.OwnerId);
                if (_model.AutoSellToMarket) trySellToMarket(tick);
                _model.Producing = false;                
                if (_model.AutoResumeProduction)
                {
                    tryStartProduction(tick, true);
                }
                else
                {                    
                    _solarSystemC.SendMessageToAgent(_model.OwnerId, new MessageAgentCommand(new MessageAgentProducerCommand(AgentCommandEnum.ProducerStoppedProducing, _bp.Consumes, _model.ProducerId, _model.PlanetScId), tick.Tick));
                }
            }
        }

        private void trySellToMarket(MessageTick tick)
        {
            foreach (ResourceQuantity rQ in _bp.Produces)
            {
                if (!_planetC.CommandForMarket(new MessageMarketCommand(new MessageMarketGeneral(MarketCommandEnum.SellToHighestBuy, rQ.Type, rQ.Quantity), _model.OwnerId, tick.Tick, _model.PlanetScId)))
                {                    
                    _planetC.CommandForMarket(new MessageMarketCommand(new MessageMarketGeneral(MarketCommandEnum.PlaceSellOrderLowest, rQ.Type, rQ.Quantity), _model.OwnerId, tick.Tick, _model.PlanetScId));
                }
            }
        }

        private void tryStartProduction(MessageTick tick, bool continuing)
        {            
            if(!tryGetResourcesAndStart(tick, continuing))
            {
                if (!_model.AutoBuyFailed && tryBuyFromMarket(tick)) tryGetResourcesAndStart(tick, continuing);                
            }
        }

        private bool tryGetResourcesAndStart(MessageTick tick, bool continuing)
        {
            if (_planetC.ResourcesRequest(_bp.Consumes, _model.OwnerId, tick.Tick))
            {
                _model.TickForNextProduction = tick.Tick + _bp.BaseTicks;
                _model.Producing = true;
                _model.AutoBuyFailed = false;
                if (_model.ProduceNThenStop > 0) _model.ProduceNThenStop--;
                if (!continuing) _solarSystemC.SendMessageToAgent(_model.OwnerId, new MessageAgentCommand(new MessageAgentProducerCommand(AgentCommandEnum.ProducerStartedProducing, null, _model.ProducerId, _model.PlanetScId), tick.Tick));
                return true;
            }
            return false;
        }

        private bool tryBuyFromMarket(MessageTick tick)
        {
            // TODO only buy resources we need
            foreach (ResourceQuantity rQ in _bp.Consumes)
            {
                if (!_planetC.CommandForMarket(new MessageMarketCommand(new MessageMarketGeneral(MarketCommandEnum.BuyFromLowestSell, rQ.Type, rQ.Quantity), _model.OwnerId, tick.Tick, _model.PlanetScId)))
                {
                    _model.AutoBuyFailed = true;
                    _planetC.CommandForMarket(new MessageMarketCommand(new MessageMarketGeneral(MarketCommandEnum.PlaceBuyOrderHighest, rQ.Type, rQ.Quantity), _model.OwnerId, tick.Tick, _model.PlanetScId));
                }
            }
            return !_model.AutoBuyFailed;
        }

        internal void receiveProducerCommand(MessageProducerCommand msg)
        {
            // we assume that run or stop always occurs after current production ends.
            if (msg.Command == ProducerCommandEnum.Run)
            {
                _model.ProduceNThenStop = msg.ProduceN;
            }
            else if (msg.Command == ProducerCommandEnum.Stop)
            {
                _model.ProduceNThenStop = 0;
            }
        }
    }

}
