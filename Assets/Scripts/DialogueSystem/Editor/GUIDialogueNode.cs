using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GUIDialogueReply : DialogueReply {
    [XmlIgnore] public ConnectionPoint outPoint;
    [XmlIgnore] public Rect removeButtonRect;
    [XmlIgnore] public GUIDialogueNode parent;
    [XmlIgnore] public int replyID;
    [XmlIgnore] public Rect rect;

    public GUIDialogueReply() {}

    public GUIDialogueReply(GUIDialogueNode parent, int verticalPos, int replyID) {
        nextLineID = -1;
        this.replyID = replyID;
        Init(parent, verticalPos);
    }

    public void Init(GUIDialogueNode parent, int verticalPos=-1) {
        this.parent = parent;
        outPoint = new ConnectionPoint(parent, ConnectionPointType.Out, this, verticalPos);
        if(verticalPos != -1) { 
            UpdateVerticalPos(verticalPos);
        }
    }

    public void UpdateVerticalPos(int verticalPos) {
        rect = new Rect(
            parent.rect.x + GUIDialogueNode.padding,
            parent.rect.y + GUIDialogueNode.mainBlockHeight + (GUIDialogueNode.textHeight + GUIDialogueNode.padding) * verticalPos,
            GUIDialogueNode.textWidth - GUIDialogueNode.buttonWidth,
            GUIDialogueNode.textHeight
        );
        removeButtonRect = new Rect(
            parent.rect.x + GUIDialogueNode.padding + rect.width,
            rect.y,
            GUIDialogueNode.buttonWidth,
            GUIDialogueNode.textHeight
        );
        outPoint.verticalPos = verticalPos;
    }

    public void Destroy() {
        foreach(var connection in outPoint.connections) {
            parent.editor.OnClickRemoveConnection(connection);
        }
    }
}

[XmlType("line")]
[XmlInclude(typeof(GUIDialogueReply))]
public class GUIDialogueNode : DialogueNode {
    [XmlIgnore] private static GUIStyle defaultNodeStyle;
    [XmlIgnore] private static GUIStyle selectedNodeStyle;

    [XmlIgnore] public static float nameHeight;
    [XmlIgnore] public static float textWidth;
    [XmlIgnore] public static float textHeight;
    [XmlIgnore] public static float buttonWidth;
    [XmlIgnore] public static float padding;
    [XmlIgnore] public static float mainBlockHeight;

    public Vector2 position;
    [XmlIgnore] public bool isInitialLine;
    [XmlIgnore] public Rect rect;
    [XmlIgnore] public Rect initialLineToggleRect;
    [XmlIgnore] public Rect nameLabelRect;
    [XmlIgnore] public Rect nameRect;
    [XmlIgnore] public Rect textLabelRect;
    [XmlIgnore] public Rect textRect;
    [XmlIgnore] public GUIStyle style;

    [XmlIgnore] public bool isDragged;
    [XmlIgnore] public bool isSelected;

    [XmlIgnore] public List<GUIDialogueReply> repliesToRemove;

    [XmlIgnore] public ConnectionPoint inPoint;
    [XmlIgnore] public DialogueEditor editor;

    public static void initStylesAndSizes() {
        defaultNodeStyle = new GUIStyle();
        defaultNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        defaultNodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        textWidth = 200;
        textHeight = 40;
        nameHeight = 20;
        padding = 10;
        buttonWidth = 20;
        mainBlockHeight = padding * 4.0f + nameHeight * 4.0f + textHeight;
    }

    public GUIDialogueNode() {

    }

    public GUIDialogueNode(DialogueEditor parentEditor, Vector2 position, int lineID) {
        this.lineID = lineID;
        this.speakerUID = parentEditor.speakerUID;
        this.position = position;
        Init(parentEditor);
    }
 
    public void Init(DialogueEditor parentEditor) {
        editor = parentEditor;
        isInitialLine = (parentEditor.initialLineID == this.lineID);
        nameLabelRect = new Rect(
            position.x + padding, position.y + padding,
            textWidth, nameHeight
        );
        nameRect = new Rect(
            position.x + padding, position.y + padding + nameHeight,
            textWidth, nameHeight
        );
        initialLineToggleRect = new Rect(
            position.x + padding, position.y + padding + nameHeight * 2.0f,
            textWidth, nameHeight
        );
        textLabelRect = new Rect(
            position.x + padding, position.y + padding + nameHeight * 3.0f,
            textWidth, textHeight
        );
        textRect = new Rect(
            position.x + padding, position.y + padding * 2.0f + nameHeight * 4.0f,
            textWidth, textHeight
        );
        rect = new Rect(
            position.x, position.y,
            textWidth + padding * 2.0f, mainBlockHeight + (textHeight + padding) * replies.Count
        );

        style = defaultNodeStyle;

        foreach(GUIDialogueReply reply in replies) {
            reply.Init(this);
        }

        inPoint = new ConnectionPoint(this, ConnectionPointType.In);
        repliesToRemove = new List<GUIDialogueReply>();
    }

    public void Drag(Vector2 delta) {
        rect.position += delta;
        initialLineToggleRect.position += delta;
        nameLabelRect.position += delta;
        nameRect.position += delta;
        textLabelRect.position += delta;
        textRect.position += delta;
        foreach(GUIDialogueReply reply in replies) {
            reply.rect.position += delta;
            reply.removeButtonRect.position += delta;
        }
    }
 
    public void Draw() {
        inPoint.Draw();
        foreach(GUIDialogueReply reply in replies) {
            reply.outPoint.Draw();
        }
        GUI.Box(rect, "", style);

        if(EditorGUI.Toggle(initialLineToggleRect, "Is first line", isInitialLine) != isInitialLine) {
            editor.SetInitialLine(lineID);
        }
        EditorGUI.LabelField(nameLabelRect, "Name:");
        if(GUI.Button(nameRect, speakerUID)) {
            GenericMenu toolsMenu = new GenericMenu();
            foreach(string uid in editor.allSpeakersUIDs) {
                toolsMenu.AddItem(new GUIContent(uid), false, () => speakerUID = uid);
            }
            toolsMenu.DropDown(new Rect(nameRect.x, nameRect.y, 0, 0));
        }

        EditorGUI.LabelField(textLabelRect, "Text:");
        text = EditorGUI.TextArea(textRect, text);

        foreach(GUIDialogueReply reply in replies) {
            reply.text = EditorGUI.TextArea(reply.rect, reply.text);
            if(GUI.Button(reply.removeButtonRect, "X")) {
                repliesToRemove.Add(reply);
            }
        }
        CleanReplies();
    }
 
    public bool ProcessEvents(Event e) {
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    if (rect.Contains(e.mousePosition)) {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    } else {
                        GUI.changed = true;
                        isSelected = true;
                        style = defaultNodeStyle;
                    }
                }
                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition)) {
                    ProcessContextMenu();
                    e.Use();
                }
                break;
 
            case EventType.MouseUp:
                isDragged = false;
                break;
 
            case EventType.MouseDrag:
                if (e.button == 0 && isDragged) {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }
        return false;
    }
        
    private void ProcessContextMenu() {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.AddItem(new GUIContent("Add reply"), false, AddReply);
        genericMenu.ShowAsContext();
    }
 
    private void AddReply() {
        rect.height += textHeight + padding;
        replies.Add(new GUIDialogueReply(this, replies.Count, replies.Count));
    }

    private void CleanReplies() {
        foreach(GUIDialogueReply reply in repliesToRemove) {
            replies.Remove(reply);
            reply.Destroy();
        }
        for(int i = 0; i < replies.Count; i++) {
            GUIDialogueReply reply = (GUIDialogueReply)replies[i];
            reply.UpdateVerticalPos(i);
        }
        rect.height -= (textHeight + padding) * repliesToRemove.Count;
        repliesToRemove.Clear();
    }

    private void OnClickRemoveNode() {
        editor.OnClickRemoveNode(this);
    }

    public bool ConnectedTo(Connection connection) {
        if(connection.inPoint == inPoint) {
            return true;
        }
        foreach(GUIDialogueReply reply in replies) {
            if(reply.outPoint == connection.outPoint) {
                return true;
            }
        }
        return false;
    }
}