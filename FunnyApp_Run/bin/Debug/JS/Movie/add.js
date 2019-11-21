var project_name="Movie";
var Path_Data ="";
var Path_Index="";
var Path_Seg="";
var max_id=1;


[[[..\\data\\default.js]]]


function init(data){
    
    Path_Data =s_file.Ini_Read(disk+"\\Net\\Web\\main.ini",project_name,"Path_Data");
    Path_Index=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini",project_name,"Path_Index");
    Path_Seg  =s_file.Ini_Read(disk+"\\Net\\Web\\main.ini",project_name,"Path_Seg");
    max_id  =s_file.Ini_Read(disk+"\\Net\\Web\\main.ini",project_name,"max_id");
    if (Path_Data=="" || Path_Index=="" || Path_Seg==""){
        set_click("");
    }
    if (max_id==""){
        max_id=0;
    }else{
        max_id=parseInt(max_id);
    }
    max_id+=1;
    s_ui.text_set("txt1",max_id);
}



function save_web(data){
    var content=s_ui.text_read("txt2");
    userName=sys_read_ini();
    
    var id=0;
    var name=s_ui.text_read("txt_name_from_web");
    var data="email="+s_string.urlencode(userName)+"&md5="+s_string.urlencode(md5)
        +"&id="+id+"&name="+s_string.urlencode(name)
        +"&content="+s_string.urlencode(content);
    //s_ui.msg(data);
    var a=s_net.http_post("http://www.funnyai.com/share/save_share.php",data);
    var strSplit=a.split(":");
    return strSplit[0];
}


function save_click(data){
    var content=s_ui.text_read("txt2");
    var ID=s_ui.text_read("txt1");
    s_file.Ini_Save(disk+"\\Net\\Web\\main.ini",project_name,"max_id",ID);
    
    var ID_From_Web=0;
    if (s_ui.checkbox_checked("ck_save_web")){
        ID_From_Web=save_web("");
    }
    var txt_name_from_web=s_ui.text_read("txt_name_from_web");
    
    
    s_sys.value_save("ID_From_Web",ID_From_Web);
    s_sys.value_save("ID",ID);
    s_sys.value_save("Name",txt_name_from_web);
    s_sys.value_save("Content",content);
    s_ui.close();
}


s_ui.textbox_init("txt2","",10,100,600,300);
s_ui.font_size("txt2",18);
s_ui.control_dock("txt2","fill");

s_ui.panel_init("panel1",0,0,500,25,"top");

s_ui.text_init("txt1","1",10,30,200,30);
s_ui.text_read_only("txt1",1);

s_ui.button_init("b_save","保存",250,30,200,30,"save_click","");


s_ui.checkbox_init("ck_save_web","保存到web",10,30,200,30);


s_ui.panel_add("panel1","ck_save_web","left");
s_ui.panel_add("panel1","b_save","left");
s_ui.panel_add("panel1","txt1","left");

/////////////////
s_ui.label_init("lb_web_name","标题：",10,30);

s_ui.text_init("txt_name_from_web","",10,30,100,30);
s_ui.panel_init("panel2",0,0,500,25,"top");
s_ui.panel_add("panel2","txt_name_from_web","fill");
s_ui.panel_add("panel2","lb_web_name","left");

/////////////////


s_ui.show_form(700,600);
s_ui.Form_Title("新建文件");

init("");



