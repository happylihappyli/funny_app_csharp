
var Path_Data ="";
var Path_Index="";
var Path_Seg="";


[[[..\\data\\common_string.js]]]



//添加回调函数
function callback_add(data){
    var id=s_sys.value_read("ID");
    var content=s_sys.value_read("Content");

    if (id!="" && content!=""){
        var file=Path_Data+"\\"+id+".txt";
        if (s_file.exists(file)) return ;
        
        s_file.save(file,content);
        
        if (id=="1"){
            s_index.Create_Start(Path_Index,true);
        }else{
            s_index.Create_Start(Path_Index,false);
        }
        s_index.Add_Document(id,content);
        s_index.Create_End();
        
        s_sys.value_save("ID","");
        s_sys.value_save("Content","");
    }
}

function index_add(data){
    //s_ui.msg(data);
    s_sys.value_save("ID","");
    s_sys.value_save("Content","");
    s_ui.Run_JS_Dialog("京东书籍\\new.js","callback_add");
}


function jd_click(data){
    //s_ui.msg(data);
    s_sys.value_save("ID","");
    s_sys.value_save("Content","");
    s_ui.Run_JS_Dialog("京东书籍\\new_jd.js","callback_add");
}

function jd_list_click(data){
    s_sys.value_save("ID","");
    s_sys.value_save("Content","");
    s_ui.Run_JS("京东书籍\\new_jd_list.js");
}


//添加回调函数
function callback_edit(data){
    var id=s_sys.value_read("ID");
    var content=s_sys.value_read("Content");

    if (id!="" && content!=""){
        s_file.save(Path_Data+"\\"+id+".txt",content);
        s_index.Create_Start(Path_Index,false);
        
        s_index.Remove_Document(id);
        
        s_index.Add_Document(id,content);
        s_index.Create_End();
        
        s_sys.value_save("ID","");
        s_sys.value_save("Content","");
    }
}

function index_edit(data){
    s_sys.value_save("ID",data);
    s_ui.Run_JS_Dialog("京东书籍\\edit.js","callback_edit");
}


function search(data){
    var seg=s_index.Seg(s_ui.text_read("txt1"));
    var result=s_index.Search(Path_Index,seg);
    
    s_xml.init(result,"doc1");
    
    var count=s_xml.count("doc1","/data/item");
    
    var html="";
    for (var i=1;i<=count;i++){
        var id=s_xml.read("doc1","/data/item[position()="+i+"]/id");
        var file=Path_Data+"\\"+id+".txt";
        var content=s_file.read(file,5);//读取5行
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
    s_ui.button_enable("b_search",1);
    s_ui.button_enable("b_add",1);
    s_ui.button_enable("b_add_jd",1);
    s_ui.button_enable("b_add_jd_list",1);
    s_ui.button_enable("b_init",1);
}

function init(data){
    
    Path_Data =s_file.Ini_Read("D:\\Net\\Web\\main.ini","book","Path_Data");
    Path_Index=s_file.Ini_Read("D:\\Net\\Web\\main.ini","book","Path_Index");
    Path_Seg  =s_file.Ini_Read("D:\\Net\\Web\\main.ini","book","Path_Seg");
    if (Path_Data=="" || Path_Index=="" || Path_Seg==""){
        set_click("");
    }
}

function set_click(data){
    s_ui.Run_JS("京东书籍\\setting.js");
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

function index_init(data){
    
    var id=s_sys.value_read("ID");
    var content=s_sys.value_read("Content");

    s_index.Create_Start(Path_Index,true);
    
    //var max_id=0;
    var Path=Path_Data;
    var strLine=s_file.File_List_File(Path);
    var strSplit=strLine.split("|");
    for(var i=0;i<strSplit.length;i++){
        var file=strSplit[i];
        if (file.endsWith(".txt")){
            var id=file.split(".")[0];
            //if (id>max_id) max_id=id;
            var content=s_file.read(Path+"\\"+file);
            s_index.Add_Document(id,content);
        }
    }
    
    s_index.Create_End();
    //s_file.Ini_Save("D:\\Net\\Web\\main.ini","book","max_id",max_id+"");
    s_ui.msg("索引生成成功！");
}


//词库初始化
s_index.Init_Seg("D:\\Funny\\FunnyAI\\Data\\Dic\\Segmentation","callback_init");

s_ui.text_init("txt1","",10,30,300,30);
s_ui.text_font_size("txt1",12);
s_ui.button_init("b_search","搜索",250,30,100,30,"search","");
s_ui.button_enable("b_search",0);


s_ui.button_init("b_add","添加",500,30,100,30,"index_add","");
s_ui.button_enable("b_add",0);
s_ui.button_init("b_add_jd_list","书List",500,30,100,30,"jd_list_click","");
s_ui.button_enable("b_add_jd_list",0);
s_ui.button_init("b_add_jd","书View",500,30,100,30,"jd_click","");
s_ui.button_enable("b_add_jd",0);


s_ui.button_init("b_init","重新生成索引",550,30,100,30,"index_init","");
s_ui.button_enable("b_init",0);

s_ui.Web_Init("web",10,130,700,500);

s_ui.Web_New_Event("web","New_URL");


s_ui.control_dock("web","fill");
//s_ui.control_dock("panel_top","top");

s_ui.panel_init("panel_top",10,10,200,30,"top");

s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Edit","编辑(&L)","edit_click","");
s_ui.Menu_Item_Add("Menu1","File","Save","保存(&S)","save_fav","");
s_ui.Menu_Add("Menu1","Tools","&Tools");
s_ui.Menu_Item_Add("Menu1","Tools","Setting","设置(&L)","set_click","");


s_ui.panel_add("panel_top","b_init","left");
s_ui.panel_add("panel_top","b_add","left");
s_ui.panel_add("panel_top","b_add_jd","left");
s_ui.panel_add("panel_top","b_add_jd_list","left");
s_ui.panel_add("panel_top","b_search","left");
s_ui.panel_add("panel_top","txt1","left");



s_ui.button_default("b_search");


s_ui.Show_Form(800,680);
s_ui.Form_Title("书籍抓取");

init("");



