using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using System.Collections.Generic;
using GalaxyGenEngine.Framework;

namespace GalaxyGen.Raylib
{
    internal class SolarSystemVis
    {

        private Viewer _renderer;        
        private ConcurrentQueue<RenderArray> _renderArrayQueue = new ConcurrentQueue<RenderArray>();
        private const int _width = 800;
        private const int _height = 800;
        private const int _scaling = 3000000;
        private const int _xOffset = _width / 2;
        private const int _yOffset = _width / 2;
        private const int _cellSize = 2;
        private RenderRectangle[] _shipRecs;
        private RenderRectangle[] _planetRecs;
        private static readonly Color FULLRED = new Color(255, 0, 0, 255);

        internal record RenderArray(byte Type, Vector2[] Positions);

        internal record RenderRectangle(int X, int Y, int W, int H);

        internal void StartVisualiser()
        {            
            _shipRecs = new RenderRectangle[0];
            _planetRecs = new RenderRectangle[0];
            _renderer = new Viewer(_width, _height, 30, "Solarsystem");
            _renderer.StartViewer(processFrame);
        }        

        internal void processFrame()
        {
            //processItemQueue();
            processArrayQueue();
            DrawRectangle(_xOffset, _yOffset, 4, 4, Color.YELLOW);
            foreach(RenderRectangle r in _shipRecs)  // foreach faster than for because we access the item multiple times
            {
                DrawRectangle(r.X, r.Y, r.W, r.H, FULLRED);                                       
            }
            foreach (RenderRectangle r in _planetRecs)  // foreach faster than for because we access the item multiple times
            {
                DrawRectangle(r.X, r.Y, r.W, r.H, Color.SKYBLUE);
            }
        }

        internal void AddRenderArray(RenderArray item)
        {
            _renderArrayQueue.Enqueue(item);
        }

        private RenderArray _rA;
        private void processArrayQueue()
        {
            while (_renderArrayQueue.TryDequeue(out _rA))
            {
                switch (_rA.Type)
                {
                    case 0:
                        updateShipsArray();
                        break;
                    case 1:
                        updatePlanetsArray();
                        break;

                }
            }
        }

        private void updateShipsArray()
        {
            if (_shipRecs.Length != _rA.Positions.Length) _shipRecs = new RenderRectangle[_rA.Positions.Length];
            for (int i = 0; i < _rA.Positions.Length; i++)
            {
                RenderRectangle r = new((int)(_rA.Positions[i].X / _scaling) + _xOffset, 
                                        (int)(_rA.Positions[i].Y / _scaling) + _yOffset, 
                                        1, 1);
                _shipRecs[i] = r;
            }
        }
        private void updatePlanetsArray()
        {
            if (_planetRecs.Length != _rA.Positions.Length) _planetRecs = new RenderRectangle[_rA.Positions.Length];
            for (int i = 0; i < _rA.Positions.Length; i++)
            {
                RenderRectangle r = new((int)(_rA.Positions[i].X / _scaling) + _xOffset,
                                        (int)(_rA.Positions[i].Y / _scaling) + _yOffset,
                                        _cellSize, _cellSize);
                _planetRecs[i] = r;
            }
        }

        //private RenderItem _r;
        //private void processItemQueue()
        //{
        //    int thisFrame = 0;
        //    while (_renderItemQueue.TryDequeue(out _r))
        //    {
        //        int X = _r.X - _xOffset;
        //        switch (_r.Type)
        //        {
        //            case 1:
        //                updateShips(X);
        //                break;
        //        }
        //        if (_r.Type != 0 && thisFrame++ >= MAX_PER_FRAME) break;
        //    }
        //}

        //private void updateShips(int X)
        //{
        //    if (_shipRecs.TryGetValue(_r.Id, out Rectangle rec))
        //    {
        //        setRectangleVals(X, ref rec);
        //        _shipRecs[_r.Id] = rec;
        //    }
        //    else
        //    {
        //        Rectangle newRec = new();
        //        setRectangleVals(X, ref newRec);
        //        _shipRecs.Add(_r.Id, newRec);
        //    }
        //}

        //private void setRectangleVals(int X, ref Rectangle rec)
        //{           
        //    rec.x = X * _cellSize;
        //    rec.y = _r.Y * _cellSize;
        //    rec.width = _cellSize;
        //    rec.height = _cellSize;
        //}
    }
}
