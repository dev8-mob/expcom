

using System;
using System.Drawing;

namespace Xperimen.Model
{
    public class ItemMenu
    {
        public string ImageIcon1 { get; set; }
        public string ImageIcon2 { get; set; }
        public string Title { get; set; }
        public Type Contentpage { get; set; }
        public bool IsSelected { get; set; }
        public Color TextMenuColor { get; set; }
    }
}
