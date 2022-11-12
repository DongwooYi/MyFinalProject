using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace UniversalAssets{
	
	public class Date{
		//START BLOCK -- attributes
		public string Month = "00";
		public string Day = "00";
		public string Year = "00";
		//END BLOCK -- attributes

		//START BLOCK -- Constructors
		public Date(){}//Empty constructor for default values

		public Date(string month, string day, string year)
		{
			Month = month;
			Day = day;
			Year = year;
		}
		//END BLOCK -- Constructors

		//START BLOCK -- Functions
		public string DateToString()
		{
			return Month + "/" + Day + "/" + Year;
		}
		//END BLOCK -- Functions
	}

	public class Task{

		//START BLOCK -- attributes
		public string TaskName = "Task Name";
		public string TaskType = "Task Type";

		public string Description = "Description";

		public string id = "";

		public string hascompleted = "";

		//Dates
		public Date AddDate = new Date("11","11","11");
		public Date GoalDate = new Date("11", "11", "11");
		public Date CompleteDate = new Date("11", "11", "11");

		//all sub tasks
		public List<SubTask> SubTasks = new List<SubTask>();

		//Misc
		public bool HasCompleted = false;
		//END BLOCK -- attributes

		//START BLOCK -- Constructors
		public Task(){}

		public Task(string taskname, string tasktype, string description, string Id, Date adddate, Date goaldate, Date completedate, string hascompleted)
		{
			TaskName = taskname;
			TaskType = tasktype;
			Description = description;
			id = Id;
			AddDate = adddate;
			GoalDate = goaldate;
			CompleteDate = completedate;
			if (hascompleted == "True") {
				HasCompleted = true;
			} else {
				HasCompleted = false;
			}
		}
		//END BLOCK -- Constructors

	}

	public class SubTask{

		//START BLOCK -- attributes
		public string TaskName = "Task Name";

		public string Description = "Description";

		//Misc
		public bool HasCompleted = false;

		public bool DescriptionDropDown = false; //if the description has been dropped down
		//END BLOCK -- attributes

		//START BLOCK -- Constructors
		public SubTask(){}

		public SubTask(string taskname, string description, string hascompleted)
		{
			TaskName = taskname;
			Description = description;
			if (hascompleted == "True") {
				HasCompleted = true;
			} else {
				HasCompleted = false;
			}
		}
		//END BLOCK -- Constructors
	}
		

	public class ManagmentWindow : EditorWindow {
		
		//START BLOCK -- attributes
		public List<Task> Tasks = new List<Task>(); //all the tasks

		public Texture2D TaskCardBackground; //background of the task
		public GUISkin Skin;

		public Vector2 Scrollpos = new Vector2(0,0);

		public Vector2 Scrollpos2 = new Vector2 (0, 0);
		public Texture2D LongButton;
		public Texture2D LongButtonPressed;

		public Texture2D Darken;

		public Texture2D TaskDoneButton;

		public Texture2D TaskCompleteTexture;

		public Texture2D Background;

		public Texture2D SubTaskCard;

		public Texture2D SubTaskDoneCard;

		public Texture2D DropDownButton;

		public Texture2D SubTaskDescription;

		public Texture2D EditIcon;

		public Texture2D DeleteIcon;

		public Texture2D AddIconUnpressed;

		public Texture2D AddIconPressed;

		public string ShowTaskType = "all";

		public bool EditingTask = false;
		public bool EditingExsistingTask = false;

		int count = 0; //The index of the task to edit

		bool isnewtask = false;

		bool hasloaded = false;

		int DisplayTaskIndex = 0; //Counter to replace "x" when rendering tasks (becuase of hidden tasks still counting towards the y offset

		float TaskOffset = 0f; //offset for when there are sub tasks and descriptions open

		bool deletemenuopen = false; //If the conformaion window to delete is open
		Task todelete; //the task to delete
		//END BLOCK -- attributes

		//START BLOCK -- Editor Window Instantiation
		[MenuItem("Split 'em Up/Tasks")] 
		public static void OpenWindow()
		{
			EditorWindow a = EditorWindow.GetWindow (typeof(ManagmentWindow), false, "Tasks");
			a.maxSize = new Vector2 (555f, 100000f);
			a.minSize = new Vector2 (555f, 1f);
		}
		//END BLOCK -- Editor Window Instantiation

		//START BLOCK -- Load Textures function
		//Loads a texture from disk
		public Texture2D LoadTextures(string path)
		{
			Texture2D tex;
			byte[] filedat;
			if (File.Exists (path)) {
				filedat = File.ReadAllBytes (path);
				tex = new Texture2D (0, 0, TextureFormat.ARGB32, false);
				tex.LoadImage (filedat);
				tex.wrapMode = TextureWrapMode.Repeat;
				return tex;
			}
			return null;
		}
		//END BLOCK -- Load Textures function

		//START BLOCK -- XML From Web
		//Makes sure the server certificate is valid and works. Ensures futureproof/compadibility with future versions of this package
		public bool ValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			bool isOk = true;
			// If there are errors in the certificate chain, look at each error to determine the cause.
			if (sslPolicyErrors != SslPolicyErrors.None) {
				for (int i = 0; i < chain.ChainStatus.Length; i++) {
					if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
						chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
						chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
						chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
						chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
						bool chainIsValid = chain.Build ((X509Certificate2)certificate);
						if (!chainIsValid) {
							isOk = false;
						}
					}
				}
			}
			return isOk;
		}

		public void GetXMLFromWeb (string url, string destination)
		{
			ServicePointManager.ServerCertificateValidationCallback = ValidationCallback;
			WebClient webcli = new WebClient ();
			webcli.DownloadFile (url, @destination);
		}
		//END BLOCK -- XMLFromWeb

		//START BLOCK -- GUI
		void OnGUI()
		{

			//START BLOCK -- variable loading for UI elements
			if (hasloaded == false) {
				Skin = Resources.Load("Main") as GUISkin;
				Background = LoadTextures ("Assets/Project Managment/Images/Background.png");
				Background.wrapMode = TextureWrapMode.Repeat;
				hasloaded = true;

				LoadFromXML ();
				TaskCardBackground = LoadTextures ("Assets/Project Managment/Images/TaskCardTransparent_v2.png");
				TaskCompleteTexture = LoadTextures ("Assets/Project Managment/Images/Task Done Card_v2.png");
				LongButton = LoadTextures ("Assets/Project Managment/Images/Button2.png");
				LongButtonPressed = LoadTextures ("Assets/Project Managment/Images/Button2Selected.png");
				Darken = LoadTextures ("Assets/Project Managment/Images/BackgroundDarken.png");
				TaskDoneButton = LoadTextures ("Assets/Project Managment/Images/TaskDoneButton.png");
				SubTaskCard = LoadTextures ("Assets/Project Managment/Images/SubTaskCard.png");
				SubTaskDoneCard = LoadTextures ("Assets/Project Managment/Images/SubTaskDoneCard.png");
				DropDownButton = LoadTextures ("Assets/Project Managment/Images/DropDownButton.png");
				SubTaskDescription = LoadTextures ("Assets/Project Managment/Images/SubTaskDescription.png");
				EditIcon = LoadTextures ("Assets/Project Managment/Images/EditIcon.png");
				DeleteIcon = LoadTextures ("Assets/Project Managment/Images/DeleteIcon.png");
				AddIconPressed = LoadTextures ("Assets/Project Managment/Images/ButtonLargePressed.png");
				AddIconUnpressed = LoadTextures ("Assets/Project Managment/Images/ButtonLargeUnpressed.png");

				hasloaded = true;
			}
			GUI.skin = Skin;
			//END BLOCK

			//START BLOCK -- Background
			GUI.DrawTexture(new Rect(0,0,Screen.width, Background.height), Background);
			//END BLOCK

			if (deletemenuopen == true) {
				GUI.TextArea (new Rect (Screen.width * 0.4f, 200f, 100f, 50f), "<b>Are you sure?</b>");
				Skin.button.normal.background = LongButton;
				Skin.button.hover.background = LongButton;
				Skin.button.onHover.background = LongButton;
				Skin.button.onActive.background = LongButton;
				Skin.button.active.background = LongButton;
				if (GUI.Button (new Rect (Screen.width * 0.3f, 250f, 50f, 50f), "Yes")) {
					Tasks.Remove (todelete);
					UpdateXML (todelete, true, todelete.id);
					deletemenuopen = false;

				}
				if (GUI.Button (new Rect (Screen.width * 0.6f, 250f, 50f, 50f), "No")) {
					deletemenuopen = false;

				}

			} else {

				//START BLOCK -- Type of tasks to show GUI. Three buttons: All, Active, Completed
				//START SUB BLOCK -- all
				if (GUI.Button (new Rect (7, 2, 172, 19), "")) {
					ShowTaskType = "all";
				}
				if (ShowTaskType == "all") {
					GUI.DrawTexture (new Rect (0, 2, 183, 20), LongButtonPressed);
				} else {
					GUI.DrawTexture (new Rect (0, 2, 183, 20), LongButton);
				}
				GUI.TextArea (new Rect (80, 2, 100, 30), "<size=12><b>All</b></size>");
				//END SUB BLOCK

				//START SUB BLOCK -- active
				if (GUI.Button (new Rect (189, 2, 168, 19), "")) {
					ShowTaskType = "active";
				}
				if (ShowTaskType == "active") {
					GUI.DrawTexture (new Rect (185, 2, 183, 20), LongButtonPressed);
				} else {
					GUI.DrawTexture (new Rect (183, 2, 183, 20), LongButton);
				}
				GUI.TextArea (new Rect (255, 2, 100, 30), "<size=12><b>Active</b></size>");
				//END SUB BLOCK

				//START SUB BLOCK -- completed
				if (GUI.Button (new Rect (373, 2, 171, 19), "")) {
					ShowTaskType = "completed";
				}
				if (ShowTaskType == "completed") {
					GUI.DrawTexture (new Rect (366, 2, 183, 20), LongButtonPressed);
				} else {
					GUI.DrawTexture (new Rect (366, 2, 183, 20), LongButton);
				}
				GUI.TextArea (new Rect (423, 2, 100, 30), "<size=12><b>Completed</b></size>");
				//END SUB BLOCK
				//END BLOCK	


				if (EditingTask == false) {

					int taskopentemp = 0;
					int subtaskscount = 0;
					for (int x = 0; x < Tasks.Count; x++) {

						//Get a count of how many sub tasks have thier drop downs open
						for (int y = 0; y < Tasks [x].SubTasks.Count; y++) {
							subtaskscount++;
							if (Tasks [x].SubTasks [y].DescriptionDropDown == true) {
								taskopentemp++;
							}
						}
					}
					//START BLOCK -- Main scroll view for task cards
					Scrollpos = GUI.BeginScrollView (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.98f, Screen.height * 0.97f), Scrollpos, new Rect (0, 0, Screen.width * 0.80f, (Tasks.Count * 175) + (subtaskscount * 35) + (taskopentemp * 100)));
					DisplayTaskIndex = 0;
					TaskOffset = 0;
					int tasksopen = 0; //count of the tasks that have thier descriptions open

					//Checks if there are no tasks
					if (Tasks.Count == 0) {
						GUI.TextArea (new Rect (0, 10, 500, 400), "<size=20>No New Tasks!</size>\nCreate a new task by pressing the '+' button on the bottom right");
					}


					//Iterate through tasks
					for (int x = 0; x < Tasks.Count; x++) {

						//Get a count of how many sub tasks have thier drop downs open
						for (int y = 0; y < Tasks [x].SubTasks.Count; y++) {
							if (Tasks [x].SubTasks [y].DescriptionDropDown == true) {
								tasksopen += 1;
							}
						}

						//Check category
						if (Tasks [x].HasCompleted == true && ShowTaskType == "active") {
							continue;
						}
						if (Tasks [x].HasCompleted == false && ShowTaskType == "completed") {
							continue;
						}

						if (Tasks [x].HasCompleted == false) {
							GUI.BeginGroup (new Rect (0, (DisplayTaskIndex * 175) + (TaskOffset * 35), 500, (tasksopen * 100) + (Tasks [x].SubTasks.Count * 35) + 150), TaskCardBackground);
						} else {
							GUI.BeginGroup (new Rect (0, (DisplayTaskIndex * 175) + (TaskOffset * 35), 500, (tasksopen * 100) + (Tasks [x].SubTasks.Count * 35) + 150), TaskCompleteTexture); //(TaskOffset*30)
						}

						//START SUB BLOCK -- Determines size of font for task name based on character length
						string TaskNameSize = "15";
						if (Tasks [x].TaskName.Length > 18) {
							TaskNameSize = "12";
						} else {
							TaskNameSize = "15";
						}
						//END SUB BLOCK

						//START SUB BLOCK -- Edit task button

						//changes button to edit icon
						Skin.button.normal.background = EditIcon;
						Skin.button.hover.background = EditIcon;
						Skin.button.onHover.background = EditIcon;
						Skin.button.onActive.background = EditIcon;
						Skin.button.active.background = EditIcon;
						if (GUI.Button (new Rect (-3, -6, 25, 25), "")) {
							count = x;
							EditingTask = true;
							EditingExsistingTask = true;
						}
						//END SUB BLOCK

						//START SUB BLOCK -- Task complete button

						//changes button to the task done checkmark
						Skin.button.normal.background = TaskDoneButton;
						Skin.button.hover.background = TaskDoneButton;
						Skin.button.onHover.background = TaskDoneButton;
						Skin.button.onActive.background = TaskDoneButton;
						Skin.button.active.background = TaskDoneButton;

						if (GUI.Button (new Rect (12, -7, 23, 23), "")) {	
							if (Tasks [x].HasCompleted == false) {
								Tasks [x].HasCompleted = true;

								//START SUB SUB BLOCK -- Date Stuff
								string sDate = DateTime.Now.ToString ();
								DateTime datval = (Convert.ToDateTime (sDate.ToString ()));
								string year = datval.Year.ToString ();
								//END SUB SUB BLOCK

								Tasks [x].CompleteDate = new Date (datval.Month.ToString (), datval.Day.ToString (), year [2].ToString () + year [3].ToString ());
								UpdateXML (Tasks [x], true, Tasks [x].id);
								UpdateXML (Tasks [x], false, Tasks [x].id);
							} else {
								Tasks [x].HasCompleted = false;
								UpdateXML (Tasks [x], true, Tasks [x].id);
								UpdateXML (Tasks [x], false, Tasks [x].id);
							}
						}
						
						//END SUB BLOCK

						//START SUB BLOCK -- Delete button
						Skin.button.normal.background = DeleteIcon;
						Skin.button.hover.background = DeleteIcon;
						Skin.button.onHover.background = DeleteIcon;
						Skin.button.onActive.background = DeleteIcon;
						Skin.button.active.background = DeleteIcon;
						if (GUI.Button (new Rect (27, -6, 21, 21), "")) {  
							deletemenuopen = true;
							todelete = Tasks [x];
							//UpdateXML (Tasks [x], true, Tasks [x].id);
							//Tasks.Remove (Tasks [x]);
						}
						//END SUB BLOCK

						//START SUB BLOCK -- Name, Description, Added date, Goal date, Complete date
						//Task Name
						GUI.TextArea (new Rect (5, 10, 200, 30), "<b><size=" + TaskNameSize + ">" + Tasks [x].TaskName + "</size></b>");

						//Task Type
						GUI.TextArea (new Rect (200, 20, 75, 20), "<b><size=10>[" + Tasks [x].TaskType + "]</size></b>");

						//Added Date
						GUI.TextArea (new Rect (368, 0, 100, 20), "<b><size=10>Added:</size></b>");
						GUI.TextArea (new Rect (435, 0, 100, 20), "<b><size=10>" + Tasks [x].AddDate.DateToString () + "</size></b>");

						//Goal Finish Date
						GUI.TextArea (new Rect (368, 12, 100, 20), "<b><size=10>Goal:</size></b>");
						GUI.TextArea (new Rect (435, 12, 100, 20), "<b><size=10>" + Tasks [x].GoalDate.DateToString () + "</size></b>");

						//Actual Finish Date
						if (Tasks [x].HasCompleted == true) {
							GUI.TextArea (new Rect (368, 24, 100, 20), "<b><size=10>Completed:</size></b>");
							GUI.TextArea (new Rect (435, 24, 100, 20), "<b><size=10>" + Tasks [x].CompleteDate.DateToString () + "</size></b>");
						}
		
						//Description Area
						GUI.TextArea (new Rect (5, 45, 460, 45), Tasks [x].Description);

						//START BLOCK -- Sub Tasks
						//Iterates through sub tasks
						int SubTaskIndex = 0;
						int SubTaskOpen = 0; //how many sub tasks have been dropped down

						for (int y = 0; y < Tasks [x].SubTasks.Count; y++) {
							TaskOffset++;
							int GroupLength = 30;
							if (Tasks [x].SubTasks [y].DescriptionDropDown == true) {
								GroupLength = 131;	
							}

							if (Tasks [x].SubTasks [y].HasCompleted == false) {
								GUI.BeginGroup (new Rect (10, (SubTaskOpen * 100) + (SubTaskIndex * 35) + 125, 490, GroupLength), SubTaskCard);
							} else {
								GUI.BeginGroup (new Rect (10, (SubTaskOpen * 100) + (SubTaskIndex * 35) + 125, 490, GroupLength), SubTaskDoneCard);
							}

							//Sub task name
							GUI.TextArea (new Rect (20, 7, 150, 30), "<b><size=12>" + Tasks [x].SubTasks [y].TaskName + "</size></b>");

							//drop down button
							Skin.button.normal.background = DropDownButton;
							Skin.button.hover.background = DropDownButton;
							Skin.button.onHover.background = DropDownButton;
							Skin.button.onActive.background = DropDownButton;
							Skin.button.active.background = DropDownButton;
							if (GUI.Button (new Rect (468, 7, 22, 22), "")) {
								if (Tasks [x].SubTasks [y].DescriptionDropDown == true) {
									Tasks [x].SubTasks [y].DescriptionDropDown = false;
								} else {
									Tasks [x].SubTasks [y].DescriptionDropDown = true;
								}
							}

							if (Tasks [x].SubTasks [y].DescriptionDropDown == true) {

								GUI.DrawTexture (new Rect (0, 30, 500, 100), SubTaskDescription);
								GUI.TextArea (new Rect (12, 32, 485, 84), Tasks [x].SubTasks [y].Description);

								//Delete Button
								Skin.button.normal.background = DeleteIcon;
								Skin.button.hover.background = DeleteIcon;
								Skin.button.onHover.background = DeleteIcon;
								Skin.button.onActive.background = DeleteIcon;
								Skin.button.active.background = DeleteIcon;
								if (GUI.Button (new Rect (470, 110, 20, 20), "")) {
									Tasks [x].SubTasks.Remove (Tasks [x].SubTasks [y]);
									UpdateXML (Tasks [x], true, Tasks [x].id);
									UpdateXML (Tasks [x], false, Tasks [x].id);
								}


								//Complete Button
								Skin.button.normal.background = TaskDoneButton;
								Skin.button.hover.background = TaskDoneButton;
								Skin.button.onHover.background = TaskDoneButton;
								Skin.button.onActive.background = TaskDoneButton;
								Skin.button.active.background = TaskDoneButton;
								if (GUI.Button (new Rect (1, 110, 22, 22), "")) {
								
									if (Tasks [x].SubTasks [y].HasCompleted == true) {
										Tasks [x].SubTasks [y].HasCompleted = false;
									} else {
										Tasks [x].SubTasks [y].HasCompleted = true;
									}
									UpdateXML (Tasks [x], true, Tasks [x].id);
									UpdateXML (Tasks [x], false, Tasks [x].id);
								}
								SubTaskOpen++;
								TaskOffset += 3.3f;
							}

							GUI.EndGroup ();

							SubTaskIndex++;
						}
						//END BLOCK -- Sub Tasks


						GUI.EndGroup ();
						//END SUB BLOCK
						DisplayTaskIndex++;
					}
					GUI.EndScrollView ();
					//END BLOCK

					//START BLOCK -- Add task button
					Skin.button.normal.background = AddIconUnpressed;
					Skin.button.hover.background = AddIconUnpressed;
					Skin.button.onHover.background = AddIconUnpressed;
					Skin.button.onActive.background = AddIconPressed;
					Skin.button.active.background = AddIconPressed;
					if (GUI.Button (new Rect (Screen.width - 50, Screen.height - 70, 50, 50), "")) {
						Tasks.Add (new Task ());
						count = Tasks.Count - 1;
						string sDate = DateTime.Now.ToString ();
						DateTime datval = (Convert.ToDateTime (sDate.ToString ()));
						string year = datval.Year.ToString ();
						Tasks [count].AddDate = new Date (datval.Month.ToString (), datval.Day.ToString (), year [2].ToString () + year [3].ToString ());
						Tasks [count].id = DateTimeOffset.Now.ToFileTime().ToString();
						EditingTask = true;
						isnewtask = true;
					}
					//END BLOCK
				}

				//START BLOCK [DEVELOPMENT] -- Makes sure there isent an out of index error when editing the script
				if (Tasks.Count == 0) {
					EditingTask = false;
				}
				//END BLOCK

				//START BLOCK -- Task Editor
				if (EditingTask == true) {

					GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Darken);


					GUI.BeginGroup (new Rect (20, 30, 800, Screen.height), TaskCardBackground);
					//Task Name
					Tasks [count].TaskName = GUI.TextArea (new Rect (5, 10, 200, 35), Tasks [count].TaskName);

					//Task type
					Tasks [count].TaskType = GUI.TextArea (new Rect (200, 20, 200, 20), Tasks [count].TaskType);

					//Added Date
					GUI.TextArea (new Rect (368, 0, 40, 15), "<b><size=10>Added:</size></b>");
					GUI.TextArea (new Rect (435, 0, 100, 15), "<b><size=10>" + Tasks [count].AddDate.DateToString () + "</size></b>");

					//Goal Finish Date
					GUI.TextArea (new Rect (368, 12, 40, 15), "<b><size=10>Goal:</size></b>");
					Tasks [count].GoalDate.Month = GUI.TextArea (new Rect (435, 12, 20, 20), Tasks [count].GoalDate.Month);
					GUI.TextArea (new Rect (450, 12, 10, 15), "/");
					Tasks [count].GoalDate.Day = GUI.TextArea (new Rect (457, 12, 20, 15), Tasks [count].GoalDate.Day);
					GUI.TextArea (new Rect (472, 12, 10, 15), "/");
					Tasks [count].GoalDate.Year = GUI.TextArea (new Rect (479, 12, 20, 15), Tasks [count].GoalDate.Year);

					//Actual Finish Date
					GUI.TextArea (new Rect (368, 24, 100, 20), "<b><size=10>Completed:</size></b>");
					GUI.TextArea (new Rect (435, 24, 100, 20), "<b><size=10>00/00/00</size></b>");

					//Description Area
					Tasks [count].Description = GUI.TextArea (new Rect (5, 45, 460, 75), Tasks [count].Description.ToString ());


					//START BLOCK -- Sub Tasks
					//Iterates through sub tasks
					int SubTaskIndex2 = 0;
					int SubTaskOpen2 = 0; //how many sub tasks have been dropped down
					int scrollviewsize = 0;
					for (int y = 0; y < Tasks [count].SubTasks.Count; y++) {
						if (Tasks [count].SubTasks [y].DescriptionDropDown == true) {
							scrollviewsize += 105;	
						} else {
							scrollviewsize += 35;
						}
					}
					Scrollpos2 = GUI.BeginScrollView (new Rect (10, 125, 500, Screen.height - 175), Scrollpos2, new Rect (8, 124, 499, scrollviewsize + 40));
					for (int y = 0; y < Tasks [count].SubTasks.Count; y++) {
						int GroupLength = 30;
						if (Tasks [count].SubTasks [y].DescriptionDropDown == true) {
							GroupLength = 131;	
						}

						if (Tasks [count].SubTasks [y].HasCompleted == false) {
							GUI.BeginGroup (new Rect (10, (SubTaskOpen2 * 100) + (SubTaskIndex2 * 35) + 125, 490, GroupLength), SubTaskCard);
						} else {
							GUI.BeginGroup (new Rect (10, (SubTaskOpen2 * 100) + (SubTaskIndex2 * 35) + 125, 490, GroupLength), SubTaskDoneCard);
						}

						//Sub task name
						Tasks [count].SubTasks [y].TaskName = GUI.TextArea (new Rect (20, 7, 150, 30), Tasks [count].SubTasks [y].TaskName);

						//drop down button
						Skin.button.normal.background = DropDownButton;
						Skin.button.hover.background = DropDownButton;
						Skin.button.onHover.background = DropDownButton;
						Skin.button.onActive.background = DropDownButton;
						Skin.button.active.background = DropDownButton;
						if (GUI.Button (new Rect (468, 7, 22, 22), "")) {
							if (Tasks [count].SubTasks [y].DescriptionDropDown == true) {
								Tasks [count].SubTasks [y].DescriptionDropDown = false;
							} else {
								Tasks [count].SubTasks [y].DescriptionDropDown = true;
							}
						}

						if (Tasks [count].SubTasks [y].DescriptionDropDown == true) {

							GUI.DrawTexture (new Rect (0, 30, 500, 100), SubTaskDescription);
							Tasks [count].SubTasks [y].Description = GUI.TextArea (new Rect (12, 32, 485, 84), Tasks [count].SubTasks [y].Description);

							//Delete Button
							Skin.button.normal.background = DeleteIcon;
							Skin.button.hover.background = DeleteIcon;
							Skin.button.onHover.background = DeleteIcon;
							Skin.button.onActive.background = DeleteIcon;
							Skin.button.active.background = DeleteIcon;
							if (GUI.Button (new Rect (470, 110, 20, 20), "")) {
								Tasks [count].SubTasks.Remove (Tasks [count].SubTasks [y]);
								UpdateXML (Tasks [count], true, Tasks [count].id);
								UpdateXML (Tasks [count], false, Tasks [count].id);
							}


							//Complete Button
							Skin.button.normal.background = TaskDoneButton;
							Skin.button.hover.background = TaskDoneButton;
							Skin.button.onHover.background = TaskDoneButton;
							Skin.button.onActive.background = TaskDoneButton;
							Skin.button.active.background = TaskDoneButton;
							if (GUI.Button (new Rect (1, 110, 22, 22), "")) {

								if (Tasks [count].SubTasks [y].HasCompleted == true) {
									Tasks [count].SubTasks [y].HasCompleted = false;
								} else {
									Tasks [count].SubTasks [y].HasCompleted = true;
								}
								UpdateXML (Tasks [count], true, Tasks [count].id);
								UpdateXML (Tasks [count], false, Tasks [count].id);
							}
							SubTaskOpen2++;
						}

						GUI.EndGroup ();

						SubTaskIndex2++;
					}
					//End of subtasks
					//Add sub task button
					Skin.button.normal.background = AddIconUnpressed;
					Skin.button.hover.background = AddIconUnpressed;
					Skin.button.onHover.background = AddIconUnpressed;
					Skin.button.onActive.background = AddIconPressed;
					Skin.button.active.background = AddIconPressed;
					if (GUI.Button (new Rect (5, (SubTaskOpen2 * 100) + (SubTaskIndex2 * 35) + 125, 25, 25), "")) {
						Tasks [count].SubTasks.Add (new SubTask ("Name", "Description", "false"));
					}

					GUI.EndScrollView ();

					GUI.EndGroup ();

					//Save Button
					if (GUI.Button (new Rect (Screen.width - 50, Screen.height - 70, 50, 50), "")) {
						EditingTask = false;
						if (isnewtask == true) {
							UpdateXML (Tasks [count], false, Tasks [count].id);
						} else {
							UpdateXML (Tasks [count], true, Tasks [count].id);
							UpdateXML (Tasks [count], false, Tasks [count].id);
						}
						if (EditingExsistingTask == false) {
							//Tasks.Reverse ();
						} else {
							EditingExsistingTask = false;
						}

					}
				}
			}

			GUI.skin = null;

		}
		//END BLOCK -- GUI

		//START BLOCK -- Save task to XML
		void UpdateXML(Task task, bool rem, string id)
		{
			
			if (rem == false){
				//creates new xml entry
				XDocument doc = XDocument.Load ("Assets/Project Managment/XML/tasks.xml");
				XElement tasks = doc.Element ("alldata").Element("tasks");
				XElement newTask = new XElement ("task", new XAttribute ("id", id), new XAttribute ("name", task.TaskName), new XAttribute ("type", task.TaskType), new XAttribute ("description", task.Description), new XAttribute ("adddate", task.AddDate.DateToString ()), new XAttribute ("goaldate", task.GoalDate.DateToString ()), new XAttribute ("completedate", task.CompleteDate.DateToString ()), new XAttribute ("hascompleted", task.HasCompleted.ToString ()));
				for (int x = 0; x < task.SubTasks.Count; x++) 
				{
					string hascom = "false";
					if (task.SubTasks [x].HasCompleted == true) {
						hascom = "True";
					}
					newTask.Add (new XElement ("subtask", new XAttribute ("name", task.SubTasks [x].TaskName), new XAttribute ("description", task.SubTasks [x].Description), new XAttribute ("hascompleted", hascom)));
				}
				tasks.Add (newTask);
				doc.Save ("Assets/Project Managment/XML/tasks.xml");
				isnewtask = false;
			} else {
					XmlDocument doc = new XmlDocument ();
					doc.Load ("Assets/Project Managment/XML/tasks.xml");
					XmlNode node = doc.SelectSingleNode ("alldata/tasks/task[@id = '" + id + "']");
				if (node != null) {
						XmlNode parent = node.ParentNode;
						parent.RemoveChild (node);
						doc.Save ("Assets/Project Managment/XML/tasks.xml");
				}
			}
		}
		//END BLOCK -- Save task to XML

		//START BLOCK -- Load task from XML
		void LoadFromXML()
		{
			XmlDocument doc = new XmlDocument ();
			doc.Load ("Assets/Project Managment/XML/tasks.xml");
			XmlNodeList record = doc.SelectNodes ("alldata/tasks/task");
			for (int x = 0; x < record.Count; x++) {
				XmlNode temprec = record [x];
				string[] adddate = temprec.Attributes ["adddate"].Value.ToString().Split(new char[]{'/'}, temprec.Attributes["adddate"].ToString().Length);
				string[] goaldate = temprec.Attributes ["goaldate"].Value.ToString().Split(new char[]{'/'}, temprec.Attributes["goaldate"].ToString().Length);
				string[] completedate = temprec.Attributes ["completedate"].Value.ToString().Split(new char[]{'/'}, temprec.Attributes["completedate"].ToString().Length);

				Task LoadTask = new Task(temprec.Attributes["name"].Value.ToString(), temprec.Attributes["type"].Value.ToString(), temprec.Attributes["description"].Value.ToString(), temprec.Attributes["id"].Value.ToString(), new Date(adddate[0], adddate[1], adddate[2]), new Date(goaldate[0], adddate[1], adddate[2]), new Date(completedate[0], completedate[1], completedate[2]), temprec.Attributes["hascompleted"].Value.ToString());
				XmlNodeList subTasks = temprec.ChildNodes;
				for (int y = 0; y < subTasks.Count; y++) {
					LoadTask.SubTasks.Add (new SubTask (subTasks [y].Attributes ["name"].Value, subTasks [y].Attributes ["description"].Value, subTasks [y].Attributes ["hascompleted"].Value));
				}
				Tasks.Add (LoadTask);
			}
			Tasks = Tasks.OrderBy (o => o.id).ToList ();
		}
		//END BLOCK -- Load task from XML


	}



	//Sticky note utility class
	public class StickyNote : EditorWindow{

		[MenuItem("Split 'em Up/Sticky Note")] 
		public static void OpenWindow()
		{
			StickyNote window = CreateInstance<StickyNote>();
			window.titleContent = new GUIContent("Sticky Note");
			window.Show();

		}

		public Texture2D LoadTextures(string path)
		{
			Texture2D tex;
			byte[] filedat;
			if (File.Exists (path)) {
				filedat = File.ReadAllBytes (path);
				tex = new Texture2D (0, 0, TextureFormat.ARGB32, false);
				tex.LoadImage (filedat);
				tex.wrapMode = TextureWrapMode.Repeat;
				return tex;
			}
			return null;
		}

		string text = "";
		bool firstload = true;
		GUISkin skin;
		Texture2D background;
		bool hasfocus = false;

		void OnGUI()
		{
			if (firstload == true) {
				firstload = false;
				skin = Resources.Load ("Main") as GUISkin;
				skin.button.normal.background = null;
				skin.button.focused.background = null;
				skin.button.active.background = null;
				skin.button.hover.background = null;
				skin.button.onHover.background = null;
				skin.button.onActive.background = null;
				skin.button.onFocused.background = null;
				skin.button.onNormal.background = null;
				background = LoadTextures("Assets/Project Managment/Images/StickyNoteBackground.png");
			}
			skin.textArea.normal.textColor = Color.black;
			skin.textArea.active.textColor = Color.black;
			skin.textArea.focused.textColor = Color.black;
			GUI.skin = skin;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);
			if (hasfocus == false) {
				GUI.TextArea (new Rect (0, 0, 250, 25), "Beware! Sticky Notes don't save!");
				if (EditorWindow.mouseOverWindow == this) {
					hasfocus = true;
				}
			} else {
				text = GUI.TextArea (new Rect (0, 0, Screen.width, Screen.height), text);
			}
			GUI.skin=null;
			skin.textArea.normal.textColor = Color.white;
			skin.textArea.active.textColor = Color.white;
			skin.textArea.focused.textColor = Color.white;
		}
	}

	//Help menu window class
	public class HelpWindow : EditorWindow
	{
		[MenuItem("Split 'em Up/Help")] 
		public static void OpenWindow()
		{
			EditorWindow.GetWindow (typeof(HelpWindow), false, "Help");
		}

		bool firstload = true;
		GUISkin skin;

		void OnGUI(){
			if (firstload == true) {
				skin = Resources.Load ("Main") as GUISkin;
				firstload = false;
			}
			skin.textArea.normal.textColor = Color.black;
			skin.textArea.active.textColor = Color.black;
			skin.textArea.focused.textColor = Color.black;
			GUI.skin = skin;
			GUI.TextArea (new Rect (0, 0, Screen.width, Screen.height), "<size=20>Help</size>\nThis documentation is ordered by window. If you have a question over how a window works, see the header with the name of the window you are looking for. If you can't find an answer to your question, or if you have a suggestion (we love suggestions!), contact support@universalassets.net.\n\n\n<size=17>Tasks</size>\nThis window helps you keep organized by creating tasks.\n\n<size=15>How to create a task</size>\nClick on the '+' button on the bottom right hand of the window. The task editor will appear.\n\n<size=15>How to create a sub-task</size>\nFrom the task editor, click the blue '+' button under the task card.\n\n<size=15>How to open the task editor</size>\nThere are 2 ways to access the task editor. The first is when you create a new task. The Task Editor will automatically open. The second is by pressing the pencil icon on the top left of the task card you want to edit.\n\n\n<size=17>Sticky Note</size>\nYou can write temporary reminders in this window.\n\n<size=15>How to create a sticky note</size>\nTo create a new sticky note, simply open the window by clicking on its name from the Split 'em Up menu.");
			skin.textArea.normal.textColor = Color.white;
			skin.textArea.active.textColor = Color.white;
			skin.textArea.focused.textColor = Color.white;
			GUI.skin = null;
		}

	}
}