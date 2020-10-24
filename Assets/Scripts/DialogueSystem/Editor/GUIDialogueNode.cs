using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// TODO inherit Node and reply
public class GUIDialogueReply {
    public ConnectionPoint outPoint;
    public Rect rect;
    public Rect removeButtonRect;
    GUIDialogueNode parent;
    public string text;
    public int replyID;

    public GUIDialogueReply(GUIDialogueNode parent, int verticalPos, int replyID) {
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
        outPoint = new ConnectionPoint(parent, ConnectionPointType.Out, verticalPos);
        this.parent = parent;
        text = "";
        this.replyID = replyID;
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

public class GUIDialogueNode {
    private static GUIStyle defaultNodeStyle;
    private static GUIStyle selectedNodeStyle;

    public static float nameHeight;
    public static float textWidth;
    public static float textHeight;
    public static float buttonWidth;
    public static float padding;
    public static float mainBlockHeight;

    public GUIStyle style;
    public Rect rect;
    public Rect nameLabelRect;
    public Rect nameRect;
    public Rect textLabelRect;
    public Rect textRect;

    public bool isDragged;
    public bool isSelected;
    public int selectedReplyText;

    public int lineID;
    public string speakerUID;
    public string text;
    public List<GUIDialogueReply> replies;
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
        mainBlockHeight = padding * 4.0f + nameHeight * 3.0f + textHeight;
    }

    public GUIDialogueNode(DialogueEditor parentEditor, Vector2 position, int lineID) {
        this.lineID = lineID;
        nameLabelRect = new Rect(
            position.x + padding, position.y + padding,
            textWidth, nameHeight
        );
        nameRect = new Rect(
            position.x + padding, position.y + padding + nameHeight,
            textWidth, nameHeight
        );
        textLabelRect = new Rect(
            position.x + padding, position.y + padding + nameHeight * 2.0f,
            textWidth, textHeight
        );
        textRect = new Rect(
            position.x + padding, position.y + padding * 2.0f + nameHeight * 3.0f,
            textWidth, textHeight
        );
        rect = new Rect(
            position.x, position.y,
            textWidth + padding * 2.0f, mainBlockHeight
        );

        style = defaultNodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In);
        replies = new List<GUIDialogueReply>();
        repliesToRemove = new List<GUIDialogueReply>();
        editor = parentEditor;
        selectedReplyText = -1;
    }
 
    public void Drag(Vector2 delta) {
        rect.position += delta;
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
        EditorGUI.LabelField(nameLabelRect, "Name:");
        speakerUID = EditorGUI.TextArea(nameRect, speakerUID);
        EditorGUI.LabelField(textLabelRect, "Text:");
        text = EditorGUI.TextArea(textRect, text);

        foreach(var reply in replies) {
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
        foreach(var reply in repliesToRemove) {
            replies.Remove(reply);
            reply.Destroy();
        }
        for(int i = 0; i < replies.Count; i++) {
            replies[i].UpdateVerticalPos(i);
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