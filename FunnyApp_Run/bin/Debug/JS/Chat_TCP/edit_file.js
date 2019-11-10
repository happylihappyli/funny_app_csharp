
var userName="none";
var log_msg="";

[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]


function event_msg(data){
    var obj=JSON.parse(data);
    var msg=obj.message;

    switch(obj.type){
        case "login.ok":
            //s_ui.msg("login.ok friend list");
            friend_list("login.ok");
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            friend_return=1;
            break;
        case "read_return":
            s_ui.text_set("txt_content",msg);
            //s_ui.msg(msg);
        case "msg":
            break;
        default:
            log_msg=s_time.Time_Now()
                +"<span style='color:red;'>"+obj.from+"</span><br>"
                +msg+"<br><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            break;
    }
}


function save_click(data){

    var token=get_token();
    var strType="save";
    var friend="robot1";
    var strContent=s_ui.text_read("txt_content");
    strContent=strContent.replaceAll("\"","\\\"");
    strContent=strContent.replaceAll("\\r","\\r");
    strContent=strContent.replaceAll("\\n","\\n");
    
    var file=s_sys.value_read("file");
    var strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"return_cmd\":\"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"file\":\""+file+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strContent+"\"}";
        
    s_tcp.send("m:<s>:"+strLine+":</s>");
}

function read_file(data){
    var token=get_token();
    var strType="read";
    var friend="robot1";
    msg_id=10000;
    var strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"return_cmd\":\"read_file\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"file\":\""+data+"\","
            +"\"to\":\""+friend+"\",\"message\":\"\"}";
        
    s_tcp.send("m:<s>:"+strLine+":</s>");
}

function read_click(data){
    
    var file=s_sys.value_read("file");
    read_file(file);
}

var file=s_sys.value_read("file");

s_ui.label_init("lb_memo","编辑文件",10,30);
s_ui.text_init("txt_file",file,10,50,500,30);

s_ui.label_init("lb_file","文件内容:",10,100);
s_ui.textbox_init("txt_content","",10,150,500,300);


s_ui.button_init("b1_read","读取",10,550,100,30,"read_click","");

s_ui.button_init("b1_save","保存",150,550,100,30,"save_click","");


//其他属性
//s_ui.button_default("b1_save");
s_ui.show_form(560,680);
s_ui.Form_Title("文件编辑");

s_tcp.hook_event("event_msg");

s_sys.tcp_event();

read_ini();

read_file(file);
