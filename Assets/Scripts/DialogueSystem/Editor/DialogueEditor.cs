using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class DialogueEditor : EditorWindow {
    // TODO resize text inputs and node itself
    // TODO serialize and deserialize

    private Rect menuBar;
    private float menuBarHeight = 20f;

    private List<GUIDialogueNode> nodes;
    private List<Connection> connections;
    
    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 drag;
    private Vector2 offset;

    [MenuItem("Window/Dialogue")]
    private static void ShowWindow() {
        var window = GetWindow<DialogueEditor>();
        window.titleContent = new GUIContent("Dialogue");
        window.Show();
    }

    private void OnEnable() {
        GUIDialogueNode.initStylesAndSizes();
        ConnectionPoint.initStyles();
    }

    private void OnGUI() {

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawMenuBar();
        DrawNodes();
        DrawConnections();
        DrawConnectionLine(Event.current);
        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);
        if (GUI.changed) {
            Repaint();
        }
        
    }

    private void DrawMenuBar()
    {
        menuBar = new Rect(0, 0, position.width, menuBarHeight);
 
        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Speaker name");
        GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35));
        GUILayout.Space(5);
        GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)  {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);
 
        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
 
        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);
 
        for (int i = 0; i < widthDivs; i++) {
            Handles.DrawLine(
                new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, 
                new Vector3(gridSpacing * i, position.height, 0f) + newOffset
            );
        }
 
        for (int j = 0; j < heightDivs; j++) {
            Handles.DrawLine(
                new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, 
                new Vector3(position.width, gridSpacing * j, 0f) + newOffset
            );
        }
 
        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes() {
        if (nodes != null) {
            foreach (GUIDialogueNode node in nodes) {
                node.Draw();
            }
        }
    }
 
    private void DrawConnections() {
        if (connections != null) {
            foreach (Connection connection in connections.ToArray()) {
                connection.Draw();
            } 
        }
    }

    private void DrawConnectionLine(Event e) {
        if (selectedInPoint != null && selectedOutPoint == null) {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
            GUI.changed = true;
        }
 
        if (selectedOutPoint != null && selectedInPoint == null) {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
 
            GUI.changed = true;
        }
    }

    private void ProcessEvents(Event e) {
        drag = Vector2.zero;
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 1) {
                    if (selectedInPoint != null || selectedOutPoint != null) {
                        ClearConnectionSelection();
                    } else {
                        ProcessContextMenu(e.mousePosition);
                    }
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)  {
                    OnDrag(e.delta);
                }
                break;
        }
    }
 
    private void ProcessNodeEvents(Event e) {
        if (nodes == null) {
            return;
        }
        for (int i = nodes.Count - 1; i >= 0; i--) {
            bool guiChanged = nodes[i].ProcessEvents(e);
            if (guiChanged) {
                GUI.changed = true;
            }
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition) {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition)); 
        genericMenu.ShowAsContext();
    }
 
    private void OnDrag(Vector2 delta) {
        drag = delta;
 
        if (nodes != null) {
            foreach (GUIDialogueNode node in nodes) {
                node.Drag(delta);
            }
        }
 
        GUI.changed = true;
    }

    public void OnClickAddNode(Vector2 mousePosition) {
        if (nodes == null) {
            nodes = new List<GUIDialogueNode>();
        }
 
        nodes.Add(new GUIDialogueNode(this, mousePosition, nodes.Count));
    }

    public void OnClickInPoint(ConnectionPoint inPoint) {
        selectedInPoint = inPoint;
 
        if (selectedOutPoint != null) {
            if (selectedOutPoint.parent != selectedInPoint.parent) {
                CreateConnection();
                ClearConnectionSelection(); 
            } else {
                ClearConnectionSelection();
            }
        }
    }
 
    public void OnClickOutPoint(ConnectionPoint outPoint) {
        selectedOutPoint = outPoint;
 
        if (selectedInPoint != null) {
            if (selectedOutPoint.parent != selectedInPoint.parent) {
                CreateConnection();
                ClearConnectionSelection();
            } else {
                ClearConnectionSelection();
            }
        }
    }

    public void OnClickRemoveNode(GUIDialogueNode node) {
        if (connections != null) {
            List<Connection> connectionsToRemove = new List<Connection>();
            // TODO optimize removing connections
            foreach (Connection connection in connections) {
                if (node.ConnectedTo(connection)) {
                    connectionsToRemove.Add(connection);
                }
            }
            foreach (Connection connection in connectionsToRemove) {
                connections.Remove(connection);
            }
            connectionsToRemove = null;
        }
        nodes.Remove(node);
    }

    private void OnClickRemoveConnection(Connection connection) {
        connections.Remove(connection);
    }
 
    private void CreateConnection() {
        if (connections == null) {
            connections = new List<Connection>();
        }
        // TODO optimize checking connections
        foreach(Connection connection in connections) {
            if (connection.outPoint == selectedOutPoint) {
                ClearConnectionSelection();
                return;
            }
        }
        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }
 
    private void ClearConnectionSelection() {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
}