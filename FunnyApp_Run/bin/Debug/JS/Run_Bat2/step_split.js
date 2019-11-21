
[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]
[[[..\\data\\run_bat_common.js]]]


var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");

function on_load(){
    var a=sys_read_ini();
    userName=a+"/linux_bat2";
    //s_ui.text_set("txt_user_name",userName);
}

function event_msg(data){
    
    var obj=JSON.parse(data);
    var msg=obj.message;

    switch(obj.type){
        case "chat_return":
            log_msg="<b>chat_return:"+obj.oid+"</b><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            s_ui.status_label_show("status_label2",msg);
            delete myMap["K"+obj.oid];
            break;
        case "msg":
            log_msg=s_time.Time_Now()
                +" <span style='color:blue;'>"+obj.from+"</span>"
                +obj.return_cmd+"<br>"+"<pre>"+msg+"</pre>"
                +"<br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            
            break;
        case "status":
            switch(obj.from){
                case "progress2":
                    var strSplit=msg.split(":");
                    s_ui.progress_show('progress2', "100",strSplit[0]);
                    break;
            }
            break;
        default:
        
            log_msg="<b>data:"+data+"</b><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
    
            break;
    }
}


function check_click(data){
    var index=parseInt(data);
    var k=index+1;
    var a=s_ui.datagrid_read("grid1",index,1);
    //s_ui.msg(a);
    if (a=="1"){
        s_file.Ini_Save(file_memo,"y","index",k);
    }
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}




function send_msg(strType,friend,msg,return_cmd){
    msg_id+=1;
    
    var token=sys_get_token();
    var strLine="";
    
    var strMsg2=msg.replaceAll("\"","\\\"");
    strMsg2=strMsg2.replaceAll("\n","\\n");
    
    if (token!=""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"return_cmd\":\""+return_cmd+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg2+"\"}";
        
        switch(strType){
            case "login":
            case "friend_list":
                break;
            default:
                myMap["K"+msg_id]=new C_Msg(msg_id,strLine);
                break;
        }
        s_tcp.send("m:<s>:"+strLine+":</s>");
        
        
        log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
                +msg+"<br><br>"+"<b>tcp.send:"+strLine+"</b>"
                +log_msg;
        s_ui.Web_Content("web",css_head+log_msg);
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }
}


function callback_do(data){
    log_msg=s_time.Time_Now()+"<b>处理完毕！</b>"
    s_ui.Web_Content("web",css_head+log_msg);
    
}


function do_click(data){
    msg_id+=1;
    var strSep=s_ui.combox_text("cb_sep0");
    var strFile1=s_ui.text_read("txt_file");
    var strFile2=s_ui.text_read("txt_output");
    var strFilter=s_ui.text_read("txt_filter");
    
    
    
    s_sys.value_save("file1",strFile1);
    s_sys.value_save("file2",strFile2);
    s_sys.value_save("sep",strSep);
    s_sys.value_save("filter",strFilter);

    
    s_sys.call_thread("sub_file_split_char.js","callback_do");
    
    
}


function file_open(){
    var strLine=s_file.File_Open();
    
    s_ui.text_set("txt_file",strLine);
}

//界面
s_ui.Web_Init("web",10,10,750,300);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.control_dock("progress2","bottom");

s_ui.panel_init("panel_middle",0,0,300,200,"bottom");

s_ui.panel_init("panel_bottom",0,0,300,50,"bottom");



s_ui.panel_init("panel_middle_left",0,0,600,200,"left");

s_ui.panel_add("panel_middle","panel_middle_left","left");



s_ui.progress_init("progress2",10,400,750,30);

s_ui.panel_add("panel_middle","progress2","bottom");

s_ui.control_dock("web","fill");


s_ui.panel_init("panel_middle_left_line1",0,0,300,30,"top");
s_ui.panel_init("panel_middle_left_line2",0,0,300,30,"top");
s_ui.panel_init("panel_middle_left_line3",0,0,300,30,"top");
s_ui.panel_init("panel_middle_left_line4",0,0,300,30,"top");

s_ui.panel_add("panel_middle_left","panel_middle_left_line1","bottom");
s_ui.panel_add("panel_middle_left","panel_middle_left_line2","bottom");
s_ui.panel_add("panel_middle_left","panel_middle_left_line3","bottom");
s_ui.panel_add("panel_middle_left","panel_middle_left_line4","bottom");




s_ui.label_init("lb_1","原分隔符：",100,320);
s_ui.combox_init("cb_sep0","|",200,320,100,30);
s_ui.combox_add("cb_sep0","|");
s_ui.combox_add("cb_sep0",",");
s_ui.combox_add("cb_sep0","空格");
s_ui.combox_add("cb_sep0","制表符");
s_ui.font_size("lb_1",18);
s_ui.font_size("cb_sep0",18);
s_ui.panel_add("panel_middle_left_line1","cb_sep0","left");
s_ui.panel_add("panel_middle_left_line1","lb_1","left");



s_ui.label_init("lb_input","输入文件：",620,100);
s_ui.text_init("txt_file","",100,100,300,530);
s_ui.button_init("b_select_file","选择文件",620,100,150,30,"file_open","");
s_ui.font_size("lb_input",18);
s_ui.font_size("txt_file",18);
s_ui.panel_add("panel_middle_left_line2","b_select_file","left");
s_ui.panel_add("panel_middle_left_line2","txt_file","left");
s_ui.panel_add("panel_middle_left_line2","lb_input","left");




s_ui.label_init("lb_output","输出文件：",620,100);
s_ui.text_init("txt_output","",100,100,300,30);
s_ui.font_size("lb_output",18);
s_ui.font_size("txt_output",18);
s_ui.panel_add("panel_middle_left_line3","txt_output","left");
s_ui.panel_add("panel_middle_left_line3","lb_output","left");



s_ui.label_init("lb_filter","过滤字符：",620,100);
s_ui.text_init("txt_filter","\"",100,100,100,30);

s_ui.font_size("lb_filter",18);
s_ui.font_size("txt_filter",18);
s_ui.panel_add("panel_middle_left_line4","txt_filter","left");
s_ui.panel_add("panel_middle_left_line4","lb_filter","left");




s_ui.button_init("b_do","处理",510,320,200,55,"do_click","");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step1");

s_ui.panel_add("panel_bottom","b_do","right");
s_ui.panel_add("panel_bottom","b_next","right");


s_ui.show_form(800,600);
s_ui.Form_Title("v2 分隔符替换");

s_sys.tcp_event();

on_load();

