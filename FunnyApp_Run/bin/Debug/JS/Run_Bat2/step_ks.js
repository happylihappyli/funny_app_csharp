

[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]


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
            switch(obj.return_cmd){
                case "step:1":
                    log_msg=s_time.Time_Now()
                        +" <span style='color:blue;'>"+obj.from+"</span>"
                        +"<br>ks="+msg
                        +"<br>"+log_msg;
                    s_ui.Web_Content("web",css_head+log_msg);
                    break;
                default:
                    log_msg=s_time.Time_Now()
                        +" <span style='color:blue;'>"+obj.from+"</span>"
                        +obj.return_cmd+"<br>"+"<pre>"+msg+"</pre>"
                        +"<br>"+log_msg;
                    s_ui.Web_Content("web",css_head+log_msg);
                    break;
            }
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
        var file=disk+"\\Net\\Web\\Data\\memo.ini";
        s_file.Ini_Save(file,"y","index",k);
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

function save_click(data){
    
    var file=disk+"\\Net\\Web\\Data\\memo.ini";
    
    var c1=s_ui.combox_index("combox_good");
    s_file.Ini_Save(file,"good","compare",c1);
    
    var good=s_ui.text_read("txt_good");
    s_file.Ini_Save(file,"good","value",good);
    
    var good_map=s_ui.combox_text("combox_good_map");
    s_file.Ini_Save(file,"good","map",good_map);
    
    var c2=s_ui.combox_index("combox_bad");
    s_file.Ini_Save(file,"bad","compare",c2);
    
    var bad=s_ui.text_read("txt_bad");
    s_file.Ini_Save(file,"bad","value",bad);
    
    var bad_map=s_ui.combox_text("combox_bad_map");
    s_file.Ini_Save(file,"bad","map",bad_map);
    
}

function static_click(data){
    msg_id+=1;
    var file1=s_sys.value_read("file1");
    if (file1=="") file1="E:\\sample1.txt";

    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    var fields_count=strSplit.length;
    //s_ui.msg(fields_count);
    var count=0;
    var file=disk+"\\Net\\Web\\Data\\memo.ini";
    for (var i=1;i<=fields_count;i++){
        var value=s_file.Ini_Read(file,"selected","check"+i);
        if (value=="1"){
            count+=1;
        }
    }
    //s_ui.msg(count);
    var id=s_ui.text_read("model_id");
    var file="/root/happyli/funny_app.jar /root/happyli/java/funny_app/ks.js";
    var file_input=s_ui.text_read("txt_file");
    var strMsg="java -jar "+file+" "+file_input+" 0 1 2";

    send_msg("cmd",friend,strMsg,"step:1");
    s_ui.text_set("txt_send","");
}


//界面
s_ui.Web_Init("web",10,10,750,390);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.label_init("lb_alg","要处理的文件:",100,420);

s_ui.text_init("txt_file","/root/data_for_static.txt",100,460,300,30);


s_ui.button_init("b_ks","ks",510,420,100,60,"static_click","");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step_static_pre");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step_roc");


s_ui.show_form(800,600);
s_ui.Form_Title("v2 ks计算");

s_sys.tcp_event();

on_load();

