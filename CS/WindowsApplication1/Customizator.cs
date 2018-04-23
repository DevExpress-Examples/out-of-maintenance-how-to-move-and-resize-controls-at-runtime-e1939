using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using DevExpress.XtraGrid;
using System.Collections;

namespace WindowsApplication1
{
    public class Customizator : Component
    {
        #region Fields

        IDesignerHost designerHost;
        Control rootDesignControl;
        DesignSurface surface;
        Control parentControl;
        Panel designPanel;

        #endregion

        #region Propeties
        private Control _SelectedControl;
        public Control SelectedControl
        {
            get {  return _SelectedControl; }
            set { IsCustomization = false; _SelectedControl = value; OnChanged(); }
        }

        private Control _DesignContainer;
        public Control DesignContainer
        {
            get { return _DesignContainer; }
            set { _DesignContainer = value; OnChanged(); }
        }

        private bool _IsCustomization;
        public bool IsCustomization
        {
            get { return _IsCustomization; }
            set
            {
                if (value && !CanCustomize(value)) return;
                { _IsCustomization = value; OnChanged(); }
            }
        }


        #endregion

        #region Methods
        void CopyBounds(Control target, Control source)
        {
            Point p = source.PointToScreen(new Point(0, 0));
            if (target.Parent != null)
            {
                p = target.Parent.PointToClient(p);
            }
            target.Location = p;
            target.Width = source.Width;
            target.Height = source.Height;
        }

        static bool IsNull(object obj)
        {
            return obj == null;
        }

        void AddToList(IList list,  Control c)
        {
            if (c.Dock != DockStyle.None) return;
            if (c.Parent == null) return;
            if (c.Name == string.Empty) return;
            list.Add(c);
        }

        void TraverseControls(IList list, Control c)
        {
            AddToList(list, c);
            foreach (Control control in c.Controls) TraverseControls(list, control);
        }

       public List<Control> GetAvailableControls()
        {
            List<Control> list = new List<Control>();
            TraverseControls(list, DesignContainer);
            return list;
        }

        
        bool NeedDestroyCustomization()
        {
            return !IsNull(rootDesignControl) && !IsNull(designPanel.Parent);
        }

        bool CanCustomize(bool isCutomization)
        {
            return !(IsNull(DesignContainer) || DesignMode || IsNull(SelectedControl) || (!isCutomization) || IsNull(SelectedControl.Parent));
        }

        Panel CreateDesignPanel()
        {
            Panel designPanel = designerHost.CreateComponent(typeof(Panel)) as Panel;
            designPanel.BackColor = Color.Black;
            designPanel.Padding = new Padding(2, 2, 2, 2);
            return designPanel;
        }

        void CreateDesignSurface()
        {
            surface = new DesignSurface();
            designerHost = surface.GetService(typeof(IDesignerHost)) as IDesignerHost;
            rootDesignControl = designerHost.CreateComponent(typeof(UserControl)) as Control;
            rootDesignControl.Dock = DockStyle.Fill;
            rootDesignControl.BackColor = Color.AliceBlue;
            Control c = surface.View as Control;
            c.Dock = DockStyle.Fill;
            c.BackColor = Color.White;
            c.Location = new Point(15, 25);
            c.Parent = DesignContainer;
        }

        void StartControlCustomization()
        {
            designPanel = CreateDesignPanel();
            rootDesignControl.Controls.Add(designPanel);
            CopyBounds(designPanel, SelectedControl);
            parentControl = SelectedControl.Parent;
            SelectedControl.Parent = designPanel;
            SelectedControl.Dock = DockStyle.Fill;
        }

        void StartCustomiztion()
        {
            if (!CanCustomize(IsCustomization)) return;
            CreateDesignSurface();
            StartControlCustomization();
        }

        void FinishControlCustomization()
        {
            SelectedControl.Parent = parentControl;
            SelectedControl.Dock = DockStyle.None;
            CopyBounds(SelectedControl, designPanel);
        }

        void EndCustomization()
        {
            if (!NeedDestroyCustomization()) return;
            FinishControlCustomization();
            DestroyDesignSurface();
        }


        void DestroyDesignSurface()
        {
            rootDesignControl.Dispose();
        }

        void OnChanged()
        {
            if (IsCustomization) StartCustomiztion();
            else EndCustomization();
        }
        #endregion
    }
}
