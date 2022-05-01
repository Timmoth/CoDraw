using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CoDraw.Client.Pages
{
    partial class Index : ComponentBase
    {
        #region Properties
        private Canvas2DContext _context;
        protected BECanvasComponent _canvasReference;
        private readonly List<List<(double x, double y)>> _lines = new();
        private List<(double x, double y)>? _currentLine;
        private (double x, double y)? _currentPos;
        private readonly int _lineResolution = 2;
        #endregion


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _context = await _canvasReference.CreateCanvas2DAsync();
            }

            await _context.BeginBatchAsync();
            await _context.SetLineWidthAsync(2);

            foreach (var line in _lines)
            {
                await _context.BeginPathAsync();
                foreach (var (x, y) in line)
                {
                    await _context.LineToAsync(x, y);
                }
                
                await _context.StrokeAsync();
            }
            await _context.EndBatchAsync();
        }

        #region Events

        public void MouseDown(MouseEventArgs e)
        {
            _currentPos = (e.OffsetX, e.OffsetY);
            _currentLine = new List<(double x, double y)>()
            {
                _currentPos.Value
            };
            _lines.Add(_currentLine);

        }

        public void MouseUp(MouseEventArgs e)
        {
            _currentLine = null;
            _currentPos = null;
        }

        public void MouseOut(MouseEventArgs e)
        {
            _currentLine = null;
            _currentPos = null;
        }

        public void MouseMove(MouseEventArgs e)
        {
            if (_currentLine == null ||
                _currentPos == null ||
                Math.Abs(e.OffsetX - _currentPos.Value.x) < _lineResolution ||
                Math.Abs(e.OffsetY - _currentPos.Value.y) < _lineResolution)
            {
                return;
            }

            _currentPos = (e.OffsetX, e.OffsetY);
            _currentLine.Add(_currentPos.Value);
        }

        public void KeyDown(KeyboardEventArgs e)
        {

        }

        public void KeyUp(KeyboardEventArgs e)
        {

        }

        #endregion

    }
}
