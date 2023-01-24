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
                requestResources(tick);
            }
        }

        private void producingTick(MessageTick tick)
        {
            if (_model.TickForNextProduction <= tick.Tick)
            {
                      
                _planetC.ReceiveProducedResource(_bp.Produces, _model.OwnerId);                    
                _model.Producing = false;
                _solarSystemC.SendMessageToAgent(_model.OwnerId, new MessageAgentCommand(new MessageAgentProducerCommand(AgentCommandEnum.ProducerStoppedProducing, _bp.Consumes, _model.ProducerId, _model.PlanetScId), tick.Tick));

                //foreach (ResourceQuantity resQ in _bp.Produces)
                //{
                //    _actorTextOutput.Tell(_model.Name + " PRODUCES " + resQ.Quantity + " " + resQ.Type.ToString() + " " + tick.Tick.ToString());
                //}


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
            if (_planetC.ResourcesRequest(_bp.Consumes, _model.OwnerId, tick.Tick))
            {
                _model.TickForNextProduction = tick.Tick + _bp.BaseTicks;
                _model.Producing = true;
                _model.ProduceNThenStop--;
                _solarSystemC.SendMessageToAgent(_model.OwnerId, new MessageAgentCommand(new MessageAgentProducerCommand(AgentCommandEnum.ProducerStartedProducing, null, _model.ProducerId, _model.PlanetScId), tick.Tick));
            }
            else
            {
                _model.Producing = false;
            }
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
