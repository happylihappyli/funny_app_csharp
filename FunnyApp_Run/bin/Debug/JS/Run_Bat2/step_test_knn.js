var friend_return=0;
var session_send=0;
var sep=1;
var step=0;//处理步骤
var row_index=0;//第几个字段被点击
var fields_count=0;

var keep_count=1;

var myMap=[];
var head="";

[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]

var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");



function check_click(data){
    var index=parseInt(data);
    var k=index+1;
    var a=s_ui.datagrid_read("grid1",index,8);
    
    s_file.Ini_Save(file_memo,"selected","check"+k,a);
}

function save_click(data){
    
    var index=parseInt(data)+1;
    var memo=s_ui.datagrid_read("grid1",index,1);
    memo=memo.replaceAll(",","");
    
    s_file.Ini_Save(file_memo,"main","memo"+index,memo);
    
    //s_ui.msg(memo);
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
            if (obj.return_cmd=="step:1"){
                log_msg=s_time.Time_Now()
                    +" <span style='color:blue;'>"+obj.from+"</span>"
                    +"<pre style='color:red;'>运行完毕,点击下一步</pre>"
                    +"<br>"+log_msg;
                s_ui.Web_Content("web",css_head+log_msg);
            }
            break;
        case "status":
            switch(obj.from){
                case "progress1":
                    var strSplit=msg.split(":");
                    s_ui.progress_show("progress1","100",strSplit[0]);
                    s_ui.progress_show("progress2","100","100");
                    break;
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




function sql(file1,sql,sep,output){
    var cmd="file_sql /root/happyli/set_hadoop.ini "+userName+" "+file1
        +" 250000 \""+sql+"\" "+sep+" "+output;
    return cmd;
}


function map_click(data){
    var index=parseInt(data);
    row_index=s_ui.datagrid_read("grid1",index,0);
    s_sys.value_save("row_index",row_index);
    
    var type=s_ui.datagrid_read("grid1",index,1);
    
    
}

function get_compare(index){
    switch(index){
        case "0":
            return ">";
        case "1":
            return ">=";
        case "2":
            return "=";
        case "3":
            return "<";
        case "4":
            return "<=";
        case "5":
            return "<>";
    }
    return "";
}

function static_click(data){
    var file1=s_file.Ini_Read(file_memo,"main","file1");
    if (file1=="") file1="E:\\sample1.txt";
    
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    fields_count=strSplit.length;
    
    
    var file2=s_file.Ini_Read(file_memo,"main","file2");
    if (file2=="") file2="/upload/sample1.txt";
    
    
    var strWhere="";
    var y_index=parseInt(s_file.Ini_Read(file_ini,"y","index"));
    var c1=s_file.Ini_Read(file_ini,"good","compare");
    var good=s_file.Ini_Read(file_ini,"good","value");
    var good_map=s_file.Ini_Read(file_ini,"good","map");
    var compare=get_compare(c1);
    strWhere="c"+y_index+compare+good;
    
    
    var id=s_file.Ini_Read(file_memo,"model","id");
    
    
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    var fields_count=strSplit.length;
    var count_field=0;
    
    for (var i=1;i<=fields_count;i++){
        var value=s_file.Ini_Read(file_memo,"selected","check"+i);
        if (value=="1"){
            count_field+=1;
        }
    }
    
    
    
    var cmd="python3 /root/happyli/python/gbdt/knn_test.py /root/test.txt "
    +id+" "+count_field+" /root/data/output.txt";
    send_msg("cmd",friend,cmd,"step:1");
}


function next_click(data){
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

function on_load(){
    var a=sys_read_ini();
    userName=a+"/linux_bat2";
    s_ui.text_set("txt_user_name",userName);
    
}

function resend_chat_msg(data) {
    for(var key in myMap){
        var pMsg=myMap[key];
        if (pMsg.Count<3){
            pMsg.Count+=1;
            s_net.Send_Msg("chat_event",pMsg.Msg);
        }else{
            var obj=JSON.parse(pMsg.Msg);
            var friend=pMsg.to;
            log_msg=s_time.Time_Now()+" <font color=red>(消息没有发送) </font> <span style='color:gray;'>"+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息丢失："+obj.message+"\r\n");
                
            
            //s_ui.Web_Content("web",css_head+log_msg);
        }
    }
}


function set_status(data){
    var strSplit=data.split(",");
    s_ui.progress_show("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    s_ui.text_set("txt_info",log_error);
}


function send_msg(strType,friend,msg,return_cmd){
    msg_id+=1;
    
    var token=sys_get_token();
    var strLine="";
    
    var strMsg2=msg.replaceAll("\"","\\\"");
    strMsg2=strMsg2.replaceAll("\n","\\n");
    
    //s_ui.msg(token);
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
        s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
            s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
        
        s_ui.Web_Content("web",css_head+log_msg);
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }
}



s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.progress_init("progress2",100,400,500,30);

s_ui.control_dock("web","fill");
s_ui.control_dock("progress2","top");


s_ui.label_init("lb_model","模型ID:",100,350);
s_ui.text_init("model_id","1",200,350,160,30);

s_ui.font_size("lb_model",18);
s_ui.font_size("model_id",18);

s_ui.panel_init("panel_middle",0,0,300,120,"bottom");


s_ui.panel_init("panel2",0,0,500,50,"bottom");

s_ui.panel_init("panel_middle_left",0,0,560,100,"left");

s_ui.panel_add("panel_middle","panel_middle_left","left");


s_ui.panel_init("panel_middle_left_line0",0,0,200,30,"top");
s_ui.panel_init("panel_middle_left_line2",0,0,200,50,"top");


s_ui.panel_add("panel_middle_left","panel_middle_left_line2","top");
s_ui.panel_add("panel_middle_left","panel_middle_left_line0","top");

s_ui.panel_add("panel_middle_left_line2","lb_model","right");
s_ui.panel_add("panel_middle_left_line2","model_id","right");



s_ui.button_init("b_test","测试",100,500,200,30,"static_click","");

s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step_test_data");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step_static_pre");


s_ui.panel_add("panel2","b_test","right");
s_ui.panel_add("panel2","b_pre","right");
s_ui.panel_add("panel2","b_next","right");



s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");

s_ui.show_form(800,600);
s_ui.Form_Title("v2 模型测试 KNN");


s_sys.tcp_event();

on_load("");


