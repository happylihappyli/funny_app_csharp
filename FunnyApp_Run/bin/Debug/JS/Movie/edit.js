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
    
    var id=s_sys.value_read("ID");
    s_ui.text_set("txt1",id);
    
    var file=Path_Data+"\\"+id+".txt";
    var content=s_file.read(file);
    s_ui.text_set("txt2",content);
    
    

    var file_ext=Path_Data+"\\"+id+".ini";//保存扩展信息
    var name=s_file.Ini_Read(file_ext,"main","name");
    s_ui.text_set("txt_name_from_web",name);
    
    var txt_id_from_web=s_file.Ini_Read(file_ext,"main","id");
    s_ui.text_set("txt_id_from_web",txt_id_from_web);
            
            
}

function save_click(data){
    var ID=s_ui.text_read("txt1");
    var ID_From_Web=s_ui.text_read("txt_id_from_web");
    var txt_name_from_web=s_ui.text_read("txt_name_from_web");
    
    s_sys.value_save("ID",ID);
    s_sys.value_save("ID_From_Web",ID_From_Web);
    s_sys.value_save("Name",txt_name_from_web);
    s_sys.value_save("Content",s_ui.text_read("txt2"));
    s_ui.close();
}

function save_web(data){
    var content=s_ui.text_read("txt2");
    userName=sys_read_ini();
    
    var id=s_ui.text_read("txt_id_from_web");
    var title=s_ui.text_read("txt_name_from_web");
    var data="email="+s_string.urlencode(userName)+"&md5="+s_string.urlencode(md5)
        +"&id="+id+"&name="+s_string.urlencode(title)
        +"&content="+s_string.urlencode(content);
    //s_ui.msg(data);
    var a=s_net.http_post("http://www.funnyai.com/share/save_share.php",data);
    s_ui.msg(a);
}


s_ui.textbox_init("txt2","",10,100,600,300);
s_ui.font_size("txt2",18);
s_ui.control_dock("txt2","fill");

s_ui.panel_init("panel1",0,0,500,25,"top");

s_ui.text_init("txt1","1",10,30,100,30);
s_ui.text_read_only("txt1",1);
s_ui.text_init("txt_id_from_web","0",10,30,100,30);
s_ui.text_read_only("txt_id_from_web",1);


s_ui.button_init("b_save","保存",250,30,200,30,"save_click","");
s_ui.button_init("b_save2","保存Web",350,30,200,30,"save_web","");

s_ui.panel_add("panel1","b_save2","left");
s_ui.panel_add("panel1","txt_id_from_web","left");
s_ui.panel_add("panel1","b_save","left");
s_ui.panel_add("panel1","txt1","left");



s_ui.label_init("lb_web_name","标题：",10,30);

s_ui.text_init("txt_name_from_web","",10,30,100,30);
s_ui.panel_init("panel2",0,0,500,25,"top");
s_ui.panel_add("panel2","txt_name_from_web","fill");
s_ui.panel_add("panel2","lb_web_name","left");



s_ui.show_form(700,600);
s_ui.Form_Title("新建文件");

init("");



