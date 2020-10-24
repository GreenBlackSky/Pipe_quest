using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
public class GUIDialogueNode
{
    private static GUIStyle defaultNodeStyle;
    private static GUIStyle selectedNodeStyle;

    private static float nameHeight;
    private static float textWidth;
    public static float textHeight;
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

    public ConnectionPoint inPoint;

    // TODO unite all this into a class
    public List<ConnectionPoint> outPoints;
    public List<Rect> replyRects;
    public List<string> replyTexts;

    public Action<GUIDialogueNode> OnRemoveNode;
    public Action<ConnectionPoint> OnClickOutPoint;

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
        mainBlockHeight = padding * 4.0f + nameHeight * 3.0f + textHeight;
    }

    public GUIDialogueNode(
        Vector2 position,
        Action<ConnectionPoint> OnClickInPoint,
        Action<ConnectionPoint> OnClickOutPoint,
        Action<GUIDialogueNode> OnClickRemoveNode
    ) {
        nameLabelRect = new Rect(
            position.x + padding,
            position.y + padding,
            textWidth,
            nameHeight
        );
        nameRect = new Rect(
            position.x + padding,
            position.y + padding + nameHeight,
            textWidth,
            nameHeight
        );
        textLabelRect = new Rect(
            position.x + padding,
            position.y + padding + nameHeight * 2.0f,
            textWidth,
            textHeight
        );
        textRect = new Rect(
            position.x + padding,
            position.y + padding * 2.0f + nameHeight * 3.0f,
            textWidth,
            textHeight
        );
        rect = new Rect(
            position.x,
            position.y,
            textWidth + padding * 2.0f, 
            mainBlockHeight
        );

        style = defaultNodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, OnClickInPoint);

        outPoints = new List<ConnectionPoint>();
        replyRects = new List<Rect>();
        replyTexts = new List<string>();

        OnRemoveNode = OnClickRemoveNode;
        this.OnClickOutPoint = OnClickOutPoint;
    }
 
    public void Drag(Vector2 delta) {
        rect.position += delta;
        nameLabelRect.position += delta;
        nameRect.position += delta;
        textLabelRect.position += delta;
        textRect.position += delta;
        for(int i = 0; i < replyRects.Count; i++) {
            Rect rect = replyRects[i];
            rect.position += delta;
            replyRects[i] = rect;
        }
    }
 
    public void Draw() {
        inPoint.Draw();
        foreach(ConnectionPoint outPoint in outPoints) {
            outPoint.Draw();
        }
        GUI.Box(rect, "", style);
        EditorGUI.LabelField(nameLabelRect, "Name:");
        EditorGUI.LabelField(textLabelRect, "Text:");
        EditorGUI.TextArea(nameRect, "");
        EditorGUI.TextArea(textRect, "");
        foreach(Rect rect in replyRects) {
            EditorGUI.TextArea(rect, "");
        }
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
        if(replyRects.Count > 0) {
            genericMenu.AddItem(new GUIContent("Remove reply"), false, RemoveReply);
        }
        genericMenu.ShowAsContext();
    }
 
    private void AddReply() {
        rect.height += textHeight + padding;

        Rect replyRect = new Rect(
            rect.x + padding,
            rect.y + mainBlockHeight + (textHeight + padding) * replyRects.Count,
            textWidth,
            textHeight
        );
        replyRects.Add(replyRect);

        ConnectionPoint outPoint = new ConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint, outPoints.Count);
        outPoints.Add(outPoint);
    }

    private void RemoveReply() {

    }

    private void OnClickRemoveNode() {
        if (OnRemoveNode != null) {
            OnRemoveNode(this);
        }
    }
}