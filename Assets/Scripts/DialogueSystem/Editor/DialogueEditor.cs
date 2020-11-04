using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class SpeakerUIDPrompt : EditorWindow {
    public static string speakerUID;
    private string newSpeakerUID = "";

    public static void ShowSpeakerUIDPrompt() {
        SpeakerUIDPrompt window = ScriptableObject.CreateInstance(typeof(SpeakerUIDPrompt)) as SpeakerUIDPrompt;
        window.position = new Rect(0, 0, 300, 40);
        window.ShowModalUtility();
    }

    void OnGUI() {
        EditorGUILayout.LabelField("Enter speaker uid:");
        newSpeakerUID = EditorGUILayout.TextArea(newSpeakerUID);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save dialogue")) {
            speakerUID = newSpeakerUID;
            Close();
        }
        if (GUILayout.Button("Cancel")) {
            speakerUID = "";
            Close();
        }
        GUILayout.EndHorizontal();
    }

    void OnInspectorUpdate() {
        Repaint();
    }
}

public class DialogueEditor : EditorWindow {
    // TODO resize text inputs and node itself
    // TODO output into console

    public string speakerUID = "new speaker";
    public string speakerName = "";
    public List<string> allSpeakersUIDs;

    private Rect menuBar;
    private float menuBarHeight = 20f;

    public int initialLineID = 0;
    private List<GUIDialogueNode> lines;
    private List<Connection> connections;
    
    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 drag;
    private Vector2 offset;

    public DialogueEditor() {
        allSpeakersUIDs = new List<string>();
    }

    [MenuItem("Window/Dialogue")]
    private static void ShowWindow() {
        var window = GetWindow<DialogueEditor>();
        window.titleContent = new GUIContent("Dialogue");
        window.Show();
    }

    private void OnEnable() {
        GUIDialogueNode.initStylesAndSizes();
        ConnectionPoint.initStyles();
        InitEditor();
        ReadSpeakers();
    }

    private void InitEditor() {
        allSpeakersUIDs = new List<string>();
        lines = new List<GUIDialogueNode>();
        connections = new List<Connection>();
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

    private void DrawMenuBar() {
        EditorStyles.textField.wordWrap = true;
        menuBar = new Rect(0, 0, position.width, menuBarHeight);

        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();
    
        if(GUILayout.Button(new GUIContent("New"), EditorStyles.toolbarButton, GUILayout.Width(60))) {
            ClearEditor();
        }
        GUILayout.Space(10);

        if(GUILayout.Button(new GUIContent("Open"), EditorStyles.toolbarButton, GUILayout.Width(60))) {
            GenericMenu toolsMenu = new GenericMenu();
            foreach(string uid in allSpeakersUIDs) {
                toolsMenu.AddItem(new GUIContent(uid), false, () => LoadDialogue(uid));
            }
            toolsMenu.DropDown(new Rect(0, 0, 0, 16));
        }
        GUILayout.Space(10);

        if(GUILayout.Button(new GUIContent("Save as"), EditorStyles.toolbarButton, GUILayout.Width(60))) {
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            SpeakerUIDPrompt.ShowSpeakerUIDPrompt();
            if(SpeakerUIDPrompt.speakerUID != "") {
                speakerUID = SpeakerUIDPrompt.speakerUID;
                SaveDialogue();
            }

            GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
        }
        GUILayout.Space(10);

        if (speakerUID == "new speaker") {
            GUI.enabled = false;
            GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(60));
            GUI.enabled = true;
        } else {
            if(GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(60))) {
                SaveDialogue();
            }
        }
 
        GUILayout.Label(speakerUID);
        speakerName = GUILayout.TextArea(speakerName);

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
        foreach (GUIDialogueNode node in lines) {
            node.Draw();
        }
    }
 
    private void DrawConnections() {
        foreach (Connection connection in connections.ToArray()) {
            connection.Draw();
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
        for (int i = lines.Count - 1; i >= 0; i--) {
            bool guiChanged = lines[i].ProcessEvents(e);
            if (guiChanged) {
                GUI.changed = true;
            }
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition) {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => AddNode(mousePosition)); 
        genericMenu.ShowAsContext();
    }
 
    private void OnDrag(Vector2 delta) {
        drag = delta;
 
        if (lines != null) {
            foreach (GUIDialogueNode node in lines) {
                node.Drag(delta);
            }
        }
 
        GUI.changed = true;
    }

    public void AddNode(Vector2 mousePosition) {
        if (lines == null) {
            lines = new List<GUIDialogueNode>();
        }

        lines.Add(new GUIDialogueNode(this, new DialogueNode(lines.Count, mousePosition)));
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

    public void RemoveNode(GUIDialogueNode node) {
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
        lines.Remove(node);
    }

    public void RemoveConnection(Connection connection) {
        connections.Remove(connection);
        connection.Destroy();
    }
 
    private void CreateConnection(ConnectionPoint inPoint=null, ConnectionPoint outPoint=null) {
        if(inPoint == null) {
            inPoint = selectedInPoint;
        }
        if(outPoint == null) {
            outPoint = selectedOutPoint;
        }
        
        // TODO optimize checking connections
        foreach(Connection connection in connections) {
            if (connection.outPoint == selectedOutPoint) {
                ClearConnectionSelection();
                return;
            }
        }
        connections.Add(new Connection(inPoint, outPoint, RemoveConnection));
    }
 
    private void ClearConnectionSelection() {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    void ClearEditor() {
        ClearConnectionSelection();
        foreach(Connection connection in connections) {
            connection.Destroy();
        }
        connections.Clear();
        lines.Clear();
        speakerName = "";
        speakerUID = "new speaker";
    }

    void ReadSpeakers() {
        allSpeakersUIDs.Clear();
        string path = @"" + "Assets/DialoguesData/";
        List<string> fileNames = new List<string>(Directory.GetFiles(path));
        foreach(string name in fileNames) {
            if(name.EndsWith(".xml")) {
                string[] parts = name.Split('/');
                string fileName = parts[parts.Length - 1].Split('.')[0];
                allSpeakersUIDs.Add(fileName);
            }
        }
    }

    void LoadDialogue(string UID) {
        ClearEditor();
        this.speakerUID = UID;
        string path = "Assets/DialoguesData/" + UID + ".xml";
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/conversation");

        speakerName = root.SelectSingleNode("fullName").InnerText;

        XmlSerializer nodeSerializer = new XmlSerializer(typeof(DialogueNode));
        XmlNode xmlLines = root.SelectSingleNode("lines");
        foreach(XmlNode lineXmlData in xmlLines.ChildNodes) {
            DialogueNode lineData = nodeSerializer.Deserialize(new XmlNodeReader(lineXmlData)) as DialogueNode;
            GUIDialogueNode node = new GUIDialogueNode(this, lineData);
            lines.Add(node);
        }
        foreach(GUIDialogueNode node in lines) {
            foreach(GUIDialogueReply reply in node.replies) {
                int lineID = reply.nextLineID;
                if(lineID != -1) {
                    CreateConnection(lines[lineID].inPoint, reply.outPoint);
                }
            }
        }
    }

    void SaveDialogue() {
        string path = "Assets/DialoguesData/" + speakerUID + ".xml";
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("conversation");
        doc.AppendChild(root);

        XmlElement xmlinitialLineUID = doc.CreateElement("initialLineID");
        xmlinitialLineUID.InnerText = initialLineID.ToString();
        root.AppendChild(xmlinitialLineUID);
    
        XmlElement xmlSpeakerFullName = doc.CreateElement("fullName");
        xmlSpeakerFullName.InnerText = speakerName;
        root.AppendChild(xmlSpeakerFullName);

        XmlElement xmlLines = doc.CreateElement("lines");
        using (XmlWriter writer = xmlLines.CreateNavigator().AppendChild()) {
            writer.WriteWhitespace("");
            XmlSerializer nodeSerializer = new XmlSerializer(typeof(DialogueNode));
            foreach (GUIDialogueNode node in lines) {
                nodeSerializer.Serialize(writer, node.nodeData);
            }
        }
        root.AppendChild(xmlLines);
        doc.Save(path);
        ReadSpeakers();
    }

    public void SetInitialLine(int lineID) {
        lines[initialLineID].isInitialLine = false;
        lines[lineID].isInitialLine = true;
        initialLineID = lineID;
    }
}
