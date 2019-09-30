
var userName="none";
var tree_map=[];
var Path="";

function Node_Click(data){
    Path=data+"\\";
    s_ui.combox_text_Set("combox1",Path);
    //s_ui.msg(strHTML);
    //s_ui.Web_Content("web",strHTML);
    
    if (tree_map[data]!=1){
        tree_map[data]=1;
        strLine=s_file.File_List_Dir(data+"\\");
        if (strLine!=""){
            strSplit=strLine.split("|");
            
            for(var i=0;i<strSplit.length;i++){
                if (strSplit[i]!="$RECYCLE.BIN"){
                    s_ui.Tree_Add_Node("tree1",data,strSplit[i],strSplit[i],"Node_Click");
                }
            }
        }
    }
    
    list_file("");
    
}

function list_file(data){
    
    var Path=s_ui.combox_text("combox1");
    var strLine=s_file.File_List_File(Path);
    var strSplit=strLine.split("|");
    var strHTML="";
    s_ui.listbox_clear("list");
    for(var i=0;i<strSplit.length;i++){
        strHTML+=strSplit[i]+"<br>";
        s_ui.listbox_add("list",strSplit[i]);
    }
}



function Node_Click2(data){
    //s_ui.msg(data);
}

function listbox_dbclick(data){
    data=s_ui.listbox_text("list");
    s_ui.Run_App(Path+"\\"+data,"");
}

function init_tree(data){
    
    s_ui.Tree_Init("tree1",10,60,200,380);
    //*
    s_ui.Tree_Add_Node_Root("tree1","root","我的收藏","Node_Click2");
    s_ui.Tree_Add_Node("tree1","root","test1","test1","Node_Click2");
    s_ui.Tree_Add_Node("tree1","root","test2","test2","Node_Click2");
    s_ui.Tree_Add_Node("tree1","root\\test1","test2","test2","Node_Click2");
    //*/
    s_ui.Tree_Add_Node_Root("tree1","C:","C:","Node_Click");
    s_ui.Tree_Add_Node_Root("tree1","D:","D:","Node_Click");
    s_ui.Tree_Add_Node_Root("tree1","E:","E:","Node_Click");
}

function init_menu(data){
    
    s_ui.Menu_Init("Menu1",0,0,800,25);
    s_ui.Menu_Add("Menu1","File","&File");
    s_ui.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
    s_ui.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");
}

function init_fav(data){
    
    s_ui.combox_add("combox1","E:\\CloudStation\\Robot5\\GitHub\\funny_app_csharp\\FunnyApp_Run\\bin\\Debug\\JS");
}

s_ui.splitcontainer_init("split",0,0,500,500,"v");

init_tree("");

s_ui.splitcontainer_add("split",0,"tree1","fill");

s_ui.listbox_init("list",250,60,450,380);
s_ui.listbox_init_event("list","listbox_dbclick")

s_ui.combox_init("combox1","",250,60,450,30);
s_ui.combox_event("combox1","list_file");

s_ui.splitcontainer_add("split",1,"list","fill");
s_ui.splitcontainer_add("split",1,"combox1","top");

init_menu("");
init_fav("");

//其他属性
s_ui.acception_button("b1_send");
s_ui.Show_Form(800,600);
s_ui.Form_Title("资源管理器");
