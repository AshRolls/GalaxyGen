using System.Collections.Concurrent;
using Raylib_cs;
using static Raylib_cs.Raylib;
using GalaxyGenEngine.Framework;
using System;

namespace GalaxyGen.Raylib
{
    internal class SolarSystemVis
    {
        private Viewer _renderer;
        private ConcurrentQueue<RenderArray> _renderArrayQueue = new ConcurrentQueue<RenderArray>();
        private const int _initialWidth = 800;
        private const int _initialHeight = 800;       
        private int _curWidth = 800;
        private int _curHeight = 800;
        private int _scalingBase = 1750;
        private int _scaling = 3000000; 
        private const int _scalingSpeed = 50;
        private System.Numerics.Vector2 _prevPos;
        private System.Numerics.Vector2 _initialPos;
        private System.Numerics.Vector2 _mouseOffset;
        private int _xOffset => (_curWidth / 2) + (int)_mouseOffset.X;
        private int _yOffset => (_curHeight / 2) + (int)_mouseOffset.Y;
        private const int _cellSize = 1;
        private RenderRectangle[] _shipRecs;
        private RenderCircle[] _planetRecs;
        private static readonly Color FULLRED = new Color(255, 0, 0, 255);

        internal record RenderArray(byte Type, Vector2[] Positions);

        internal record RenderRectangle(int X, int Y, int W, int H);
        internal record RenderCircle(int X, int Y, int R);

        internal void StartVisualiser()
        {            
            _shipRecs = new RenderRectangle[0];
            _planetRecs = new RenderCircle[0];
            _renderer = new Viewer(_initialWidth, _initialHeight, 60, "Solarsystem");
            _renderer.StartViewer(processFrame);
        }        

        internal void processFrame()
        {
            //processItemQueue();
            processArrayQueue(); 
            if (IsWindowResized())
            {
                _curWidth = GetScreenWidth();
                _curHeight = GetScreenHeight();
            }

            DrawCircle(_xOffset, _yOffset, _cellSize * 2, Color.YELLOW);
            foreach(RenderRectangle r in _shipRecs)  // foreach faster than for because we access the item multiple times
            {
                DrawPixel(r.X, r.Y, FULLRED);                                       
            }
            foreach (RenderCircle r in _planetRecs)  // foreach faster than for because we access the item multiple times
            {
                DrawCircle(r.X, r.Y, r.R, Color.SKYBLUE);
            }
            processMouse();
        }

        private void processMouse()
        {
            if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
            {
                _initialPos = GetMousePosition();
                _prevPos = System.Numerics.Vector2.Zero;
            }
            else if (IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                System.Numerics.Vector2 thisPos = GetMousePosition() - _initialPos;
                System.Numerics.Vector2 delta = thisPos - _prevPos;              
                _prevPos = thisPos;
                _mouseOffset += delta;
            }
            _scalingBase = Math.Max((_scalingBase - (int)(GetMouseWheelMove() * _scalingSpeed)), 0);
            _scaling = _scalingBase * _scalingBase * 2;
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
            if (_planetRecs.Length != _rA.Positions.Length) _planetRecs = new RenderCircle[_rA.Positions.Length];
            for (int i = 0; i < _rA.Positions.Length; i++)
            {
                RenderCircle r = new(   (int)(_rA.Positions[i].X / _scaling) + _xOffset,
                                        (int)(_rA.Positions[i].Y / _scaling) + _yOffset,
                                        _cellSize);
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
