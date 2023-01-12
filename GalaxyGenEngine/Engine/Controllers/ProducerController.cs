using Akka.Actor;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Model;
using GalaxyGenCore.BluePrints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class ProducerController
    {
        private Producer _model;
        private BluePrint _bp;
        private PlanetController _planetC;
        private TextOutputController _textOutput;

        public ProducerController(Producer p, PlanetController pc, TextOutputController textOutput)
        {
            _model = p;
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
                requestResources(tick);
            }
        }

        private void producingTick(MessageTick tick)
        {
            if (_model.TickForNextProduction <= tick.Tick)
            {
                if (_model.Owner != null)
                {
                    MessageProducedResources mpr = new MessageProducedResources(_bp.Produces, _model.Owner);
                    _planetC.ReceiveProducedResource(mpr);                    
                    _model.Producing = false;

                    //foreach (ResourceQuantity resQ in _bp.Produces)
                    //{
                    //    _actorTextOutput.Tell(_model.Name + " PRODUCES " + resQ.Quantity + " " + resQ.Type.ToString() + " " + tick.Tick.ToString());
                    //}
                }

                if (_model.AutoResumeProduction)
                {
                    requestResources(tick);
                }
                else
                {
                    _model.Producing = false;
                }
            }
        }

        private void requestResources(MessageTick tick)
        {
            MessagePlanetRequestResources msg = new MessagePlanetRequestResources(_bp.Consumes, _model.Owner.AgentId, _model.Planet.Stores[_model.Owner.AgentId].StoreId, tick.Tick);
            if (_planetC.ReceiveResourceRequest(msg))
            {
                _model.TickForNextProduction = msg.TickSent + _bp.BaseTicks;
                _model.Producing = true;
                _model.ProduceNThenStop--;
            }
            else
            {
                _model.Producing = false;
            }
        }

        internal void receiveProducerCommand(MessageProducerCommand msg)
        {
            // we assume that run or stop always occurs after current production ends.
            if (msg.ProducerCommand == ProducerCommand.Run)
            {
                _model.ProduceNThenStop = msg.ProduceN;
            }
            else if (msg.ProducerCommand == ProducerCommand.Stop)
            {
                _model.ProduceNThenStop = 0;
            }
        }
    }

}
