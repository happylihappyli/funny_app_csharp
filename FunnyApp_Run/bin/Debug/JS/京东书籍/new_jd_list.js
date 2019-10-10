
var Path_Data ="";
var Path_Index="";
var Path_Seg="";

[[[..\\data\\common_string.js]]]

function init(data){
    
    Path_Data =s_file.Ini_Read("D:\\Net\\Web\\main.ini","book","Path_Data");
    Path_Index=s_file.Ini_Read("D:\\Net\\Web\\main.ini","book","Path_Index");
    Path_Seg  =s_file.Ini_Read("D:\\Net\\Web\\main.ini","book","Path_Seg");
    if (Path_Data=="" || Path_Index=="" || Path_Seg==""){
        set_click("");
    }
}

function save_click(data){
    var ID=s_ui.text_read("txt1");
    //s_file.Ini_Save("D:\\Net\\Web\\main.ini","book","max_id",ID);
    s_sys.value_save("ID",ID);
    s_sys.value_save("Content",s_ui.text_read("txt_content"));
    s_ui.close();
}

function crawl_list_click(data){
    var page="";
    var file="d:\\list.txt";

    var url=s_ui.text_read("txt_url");
    
    //*
    page=s_net.http_get(url,"gb2312");
    //s_ui.msg(page.substr(0,1000));
    s_file.save_encode(file,page,"utf-8");
    
    
    //page=s_file.read(file);
    
    var array;
    var regexp=new RegExp('"url":"(\\\\/\\\\/item.jd.com\\\\/(.*?).html)"','g');
    
    var html="";
    while((array=regexp.exec(page))!=null){
        var url=array[1];
        url=url.replaceAll("\\\\","");
        s_ui.listbox_add("txt_content",url);
    }
}

//view 回调函数
function callback_view(data){
    s_ui.listbox_remove("txt_content",0);
    
    var id=s_sys.value_read("ID");//ISBN
    var content=s_sys.value_read("Content");

    if (id!="" && content!=""){
        var file=Path_Data+"\\"+id+".txt";
        //s_ui.msg(file);
        //s_ui.msg(content.substr(1,1000));
        s_file.save(file,content);
        if (id=="1"){
            s_index.Create_Start(Path_Index,true);
        }else{
            s_index.Create_Start(Path_Index,false);
        }
        //s_ui.msg("Create_Start");
        s_index.Add_Document(id,content);
        //s_ui.msg("Add_Document");
        s_index.Create_End();
        //s_ui.msg("Create_End");
        
        s_sys.value_save("ID","");
        s_sys.value_save("Content","");
        //s_ui.msg("clear");
    }
    
    s_sys.value_save("action","idle");
    
}

function view_click(data){
    s_sys.value_save("action","view_click");
    
    if (s_ui.listbox_item_size("txt_content")>0){
        var url=s_ui.listbox_item("txt_content",0);
        
        var auto="0";
        if (s_ui.checkbox_checked("ck_auto")) auto="1";
        
        
        s_sys.value_save("action","get_url");
        s_sys.value_save("auto",auto);
        s_sys.value_save("url",url);
        
        s_ui.Run_JS_Dialog("京东书籍\\new_jd.js","callback_view");
    }
}

function check_auto(data){
    s_time.setTimeout("check_auto",3,"check_auto");
    
    if (s_ui.checkbox_checked("ck_auto")){
        var action=s_sys.value_read("action");
        if (action=="idle"){
            s_sys.value_save("action","");
            view_click("");
        }
    }
}

s_ui.listbox_init("txt_content",10,100,600,300);
s_ui.Control_Dock("txt_content","fill");

s_ui.panel_init("panel1",0,0,500,30,"top");
s_ui.panel_init("panel2",0,0,500,30,"top");

s_ui.text_init("txt_url","https://book.jd.com/children.html",10,30,500,30);

s_ui.checkbox_init("ck_auto","自动",250,30,200,30);
s_ui.button_init("b_crawl_list","抓List",250,30,100,30,"crawl_list_click","");
s_ui.button_init("b_crawl","View",250,30,100,30,"view_click","");


s_ui.text_font_size("txt_url",12);

s_ui.panel_add("panel1","ck_auto","left");

s_ui.panel_add("panel2","b_crawl","left");
s_ui.panel_add("panel2","b_crawl_list","left");
s_ui.panel_add("panel2","txt_url","left");



s_ui.Show_Form(750,600);
s_ui.Form_Title("抓取京东书籍");

init("");
check_auto("");



