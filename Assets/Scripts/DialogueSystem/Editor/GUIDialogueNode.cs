using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GUIDialogueReply {
    public ConnectionPoint outPoint;
    public Rect removeButtonRect;
    public GUIDialogueNode parent;
    public Rect rect;

    public int replyID;
    public DialogueReply replyData;
    public string text {
        get {return replyData.text;}
        set {replyData.text = value;}
    }
    public int nextLineID {
        get {return replyData.nextLineID;}
        set {replyData.nextLineID = value;}
    }

    public GUIDialogueReply(DialogueReply data, GUIDialogueNode parent, int replyID, int verticalPos = -1) {
        replyData = data;
        this.replyID = replyID;
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
            parent.editor.RemoveConnection(connection);
        }
    }
}

public class GUIDialogueNode {
    private static GUIStyle defaultNodeStyle;
    private static GUIStyle selectedNodeStyle;

    public static float nameHeight;
    public static float textWidth;
    public static float textHeight;
    public static float buttonWidth;
    public static float padding;
    public static float mainBlockHeight;

    public DialogueNode nodeData;

    public int lineID {
        get {return this.nodeData.lineID;}
        set {this.nodeData.lineID = value;}
    }
    public string speakerUID {
        get {return this.nodeData.speakerUID;}
        set {this.nodeData.speakerUID = value;}
    }
    public string text {
        get {return this.nodeData.text;}
        set {this.nodeData.text = value;}
    }
    public List<GUIDialogueReply> replies;

    public bool isInitialLine;
    public Rect rect;
    public Rect initialLineToggleRect;
    public Rect nameLabelRect;
    public Rect nameRect;
    public Rect textLabelRect;
    public Rect textRect;
    public GUIStyle style;

    public bool isDragged;
    public bool isSelected;

    public List<GUIDialogueReply> repliesToRemove;

    public ConnectionPoint inPoint;
    public DialogueEditor editor;

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

    public GUIDialogueNode(DialogueEditor parentEditor, DialogueNode data) {
        this.nodeData = data;
        this.speakerUID = parentEditor.speakerUID;
        this.editor = parentEditor;
        this.replies = new List<GUIDialogueReply>();
        if(nodeData.replies.Count == 0) {
            nodeData.replies.Add(new DialogueReply());
        }
        for(int i = 0; i < data.replies.Count; i++) {
            DialogueReply replyData = data.replies[i];
            this.replies.Add(new GUIDialogueReply(replyData, this, i, i));
        }
        repliesToRemove = new List<GUIDialogueReply>();
        
        isInitialLine = (parentEditor.initialLineID == this.lineID);
        nameLabelRect = new Rect(
            data.position.x + padding, data.position.y + padding,
            textWidth, nameHeight
        );
        nameRect = new Rect(
            data.position.x + padding, data.position.y + padding + nameHeight,
            textWidth, nameHeight
        );
        initialLineToggleRect = new Rect(
            data.position.x + padding, data.position.y + padding + nameHeight * 2.0f,
            textWidth, nameHeight
        );
        textLabelRect = new Rect(
            data.position.x + padding, data.position.y + padding + nameHeight * 3.0f,
            textWidth, textHeight
        );
        textRect = new Rect(
            data.position.x + padding, data.position.y + padding * 2.0f + nameHeight * 4.0f,
            textWidth, textHeight
        );
        rect = new Rect(
            data.position.x, data.position.y,
            textWidth + padding * 2.0f, mainBlockHeight + (textHeight + padding) * replies.Count
        );

        style = defaultNodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In);
    }

    public void Drag(Vector2 delta) {
        nodeData.position += delta;
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
            if(GUI.Button(reply.removeButtonRect, "X") && replies.Count > 1) {
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
        DialogueReply reply = new DialogueReply();
        nodeData.replies.Add(reply);
        replies.Add(new GUIDialogueReply(reply, this, replies.Count, replies.Count));
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
        editor.RemoveNode(this);
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
