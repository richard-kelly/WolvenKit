﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using W3Edit.CR2W.Types;
using W3Edit.CR2W;

namespace W3Edit.FlowTreeEditors
{
    public partial class SceneSectionEditor : SceneLinkEditor
    {
        public SceneSectionEditor()
        {
            InitializeComponent();
        }

        private List<Label> lines;

        public override void UpdateView()
        {
            base.UpdateView();

            if (lines != null)
            {
                foreach(var l in lines)
                {
                    Controls.Remove(l);
                }
            }

            lines = new List<Label>();

            var y = 21;
            var line = 0;

            var sceneElementsObj = Chunk.GetVariableByName("sceneElements");
            if (sceneElementsObj != null && sceneElementsObj is CArray)
            {
                var sceneElements = (CArray)sceneElementsObj;
                foreach (var element in sceneElements)
                {
                    if (element != null && element is CPtr)
                    {
                        var ptr = (CPtr)element;
                        switch (ptr.PtrTargetType)
                        {
                            case "CStorySceneLine":
                                line++;
                                var label = new Label()
                                {
                                    Width = Width,
                                    Height = 20,
                                    Location = new Point(0, y),
                                    AutoSize = false,
                                    Text = GetDisplayString(ptr.PtrTarget),
                                };
                                lines.Add(label);
                                Controls.Add(label);

                                var texts = TextRenderer.MeasureText(label.Text, label.Font, new Size(Width-6, 100), TextFormatFlags.WordBreak);
                                label.Height = texts.Height + 5;
                                label.BackColor = (line%2) == 0 ?Color.LightBlue : Color.Transparent;

                                label.Click += delegate(object sender, EventArgs e)
                                {
                                    FireSelectEvent(ptr.PtrTarget);
                                };

                                y += label.Height;

                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            Height = y;
        }

        private string GetDisplayString(CR2WChunk c)
        {
            var str = "";
            if (c != null)
            {
                var speaker = c.GetVariableByName("voicetag");
                if(speaker != null && speaker is CName)
                {
                    str += ((CName)speaker).Value + ": ";
                }

                var line = c.GetVariableByName("dialogLine");
                if(line != null && line is CLocalizedString)
                {
                    str += ((CLocalizedString)line).Text;
                }
            }

            return str;
        }

        public override List<CPtr> GetConnections()
        {
            var list = new List<CPtr>();

            var choiceObj = Chunk.GetVariableByName("choice");
            if (choiceObj != null && choiceObj is CPtr)
            {
                var choicePtr = ((CPtr)choiceObj);
                if (choicePtr.PtrTarget != null)
                {
                    list.Add(choicePtr);
                }
            }


            var nextLinkElementObj = Chunk.GetVariableByName("nextLinkElement");
            if (nextLinkElementObj != null && nextLinkElementObj is CPtr)
            {
                var nextLinkElementPtr = ((CPtr)nextLinkElementObj);
                if (nextLinkElementPtr.PtrTarget != null)
                {
                    list.Add(nextLinkElementPtr);
                }
            }

            return list;
        }

        public override string GetCopyText()
        {
            var text = new StringBuilder();
            foreach(var line in lines) {
                text.AppendLine(line.Text);
            }

            return text.ToString();
        }
    }
}
