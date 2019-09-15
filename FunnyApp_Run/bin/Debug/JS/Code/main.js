
var Path_Data ="";
var Path_Index="";
var Path_Seg="";


[[[..\\data\\common_string.js]]]



function index_create(data){
    s_index.Create_Start(Path_Index,true);
    s_index.Add_Document("1","你好中国");
    s_index.Add_Document("2","中国社会发展");
    s_index.Create_End();
}


//添加回调函数
function callback_add(data){
    var id=s_sys.Value_Read("ID");
    var content=s_sys.Value_Read("Content");

    if (id!="" && content!=""){
        s_file.save(Path_Data+"\\"+id+".txt",content);
        if (id=="1"){
            s_index.Create_Start(Path_Index,true);
        }else{
            s_index.Create_Start(Path_Index,false);
        }
        s_index.Add_Document(id,content);
        s_index.Create_End();
        
        s_sys.Value_Save("ID","");
        s_sys.Value_Save("Content","");
    }
}

function index_add(data){
    //s_ui.Msg(data);
    s_sys.Value_Save("ID","");
    s_sys.Value_Save("Content","");
    s_ui.Run_JS_Dialog("Code/new.js","callback_add");
}


//添加回调函数
function callback_edit(data){
    var id=s_sys.Value_Read("ID");
    var content=s_sys.Value_Read("Content");

    if (id!="" && content!=""){
        s_file.save(Path_Data+"\\"+id+".txt",content);
        if (id=="1"){
            s_index.Create_Start(Path_Index,true);
        }else{
            s_index.Create_Start(Path_Index,false);
        }
        
        s_index.Remove_Document(id);
        
        s_index.Add_Document(id,content);
        s_index.Create_End();
        
        s_sys.Value_Save("ID","");
        s_sys.Value_Save("Content","");
    }
}

function index_edit(data){
    s_sys.Value_Save("ID",data);
    s_ui.Run_JS_Dialog("Code/edit.js","callback_edit");
}


function search(data){
    var seg=s_index.Seg(s_ui.Text_Read("txt1"));
    var result=s_index.Search(Path_Index,seg);
    
    s_xml.init(result,"doc1");
    
    var count=s_xml.count("doc1","/data/item");
    
    var html="";
    for (var i=1;i<=count;i++){
        var id=s_xml.read("doc1","/data/item[position()="+i+"]/id");
        var file=Path_Data+"\\"+id+".txt";
        var content=s_file.read(file);
        content=content.replaceAll("\n","<br>");
        content=content.substr(0,500);
        //s_xml.read("doc1","/data/item[position()="+i+"]/content");
        var file2=file.replaceAll("\\\\","/");
        var link="<a href='http://view.com/?file="+file+"' target=_blank>打开</a> &nbsp; "
        +"<a href='http://edit.com/?file="+id+"' target=_blank>编辑</a>";
        html+="id="+id+"<br>"+content+"<br>"+link+"<hr>";
    }
    s_ui.Web_Content("web",html);
}

function callback_init(data){
    s_ui.Button_Enable("b_search",1);
    s_ui.Button_Enable("b_add",1);
}

function init(data){
    
    Path_Data =s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Data");
    Path_Index=s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Index");
    Path_Seg  =s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Seg");
    if (Path_Data=="" || Path_Index=="" || Path_Seg==""){
        set_click("");
    }
}

function set_click(data){
    s_ui.Run_JS("Code\\setting.js");
}

function New_URL(data){
    var strSplit=data.split("?");
    var file=strSplit[1];
    switch(strSplit[0]){
        case "http://view.com/":
            var file=file.substr(5);
            var html=s_file.read(file);
            html=html.replaceAll("\n","<br>");
            s_ui.Web_Content("web",html);
            break;
        case "http://edit.com/":
            var id=file.substr(5);
            index_edit(id);
            break;
    }
}


//词库初始化
s_index.Init_Seg("D:\\Funny\\FunnyAI\\Data\\Dic\\Segmentation","callback_init");

s_ui.Text_Init("txt1","",10,30,300,30);
s_ui.Text_Font_Size("txt1",12);
s_ui.Button_Init("b_search","搜索",250,30,100,30,"search","");
s_ui.Button_Enable("b_search",0);


s_ui.Button_Init("b_add","添加",500,30,100,30,"index_add","");
s_ui.Button_Enable("b_add",0);

s_ui.Web_Init("web",10,130,700,500);

s_ui.Web_New_Event("web","New_URL");


s_ui.Control_Dock("web","fill");
//s_ui.Control_Dock("panel_top","top");

s_ui.Panel_Init("panel_top",10,10,200,30,"top");

s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Edit","编辑(&L)","edit_click","");
s_ui.Menu_Item_Add("Menu1","File","Save","保存(&S)","save_fav","");
s_ui.Menu_Add("Menu1","Tools","&Tools");
s_ui.Menu_Item_Add("Menu1","Tools","Setting","设置(&L)","set_click","");


s_ui.Panel_Add("panel_top","b_add","left");
s_ui.Panel_Add("panel_top","b_search","left");
s_ui.Panel_Add("panel_top","txt1","left");



s_ui.Button_Default("b_search");


s_ui.Show_Form(800,680);
s_ui.Form_Title("代码收藏库");

init("");



