using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
public class GUIDialogueNode
{
    public Rect rect;
    public Rect textRect;
    public float heightStep;
    public string title;
    public bool isDragged;
    public bool isSelected;
 
    public ConnectionPoint inPoint;
    public List<ConnectionPoint> outPoints;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public GUIStyle outPointStyle;

    public Action<GUIDialogueNode> OnRemoveNode;
    public Action<ConnectionPoint> OnClickOutPoint;

    public GUIDialogueNode(
        Vector2 position, float width, float height, float xPadding, float yPadding,
        GUIStyle nodeStyle, GUIStyle selectedStyle, 
        GUIStyle inPointStyle, GUIStyle outPointStyle, 
        Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<GUIDialogueNode> OnClickRemoveNode
    ) {   
        rect = new Rect(position.x, position.y, width, height);
        textRect = new Rect(
            position.x + xPadding,
            position.y + yPadding,
            width - 2 * xPadding,
            height - 2 * yPadding
        );

        heightStep = height;
        style = nodeStyle;
        this.outPointStyle = outPointStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, 0, OnClickInPoint);
        outPoints = new List<ConnectionPoint>();
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
        this.OnClickOutPoint = OnClickOutPoint;
        
    }
 
    public void Drag(Vector2 delta) {
        rect.position += delta;
        textRect.position += delta;
    }
 
    public void Draw() {
        inPoint.Draw();
        foreach(ConnectionPoint outPoint in outPoints) {
            outPoint.Draw();
        }
        GUI.Box(rect, title, style);
        EditorGUI.TextArea(textRect, "");
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
        rect.height += heightStep;
        ConnectionPoint outPoint = new ConnectionPoint(
            this, 
            ConnectionPointType.Out, 
            outPointStyle, 
            outPoints.Count,
            OnClickOutPoint
        );
        outPoints.Add(outPoint);
    }

    private void OnClickRemoveNode() {
        if (OnRemoveNode != null) {
            OnRemoveNode(this);
        }
    }
}