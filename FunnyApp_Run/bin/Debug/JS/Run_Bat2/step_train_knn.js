

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
            
            
            log_msg=s_time.Time_Now()
                +" <span style='color:blue;'>"+obj.from+"</span>"
                +"<pre style='color:red;'>训练完毕！</pre>"
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


function train_click(data){
    msg_id+=1;
    var count_field=sys_field_selected();
    var id=s_ui.text_read("model_id");
    s_file.Ini_Save(file_memo,"model","id",id);
    
    var count_neightors=s_ui.text_read("tx_count");
    var strMsg=python_bin+" /root/happyli/python/gbdt/knn_train.py /root/data/train.txt "
                +id+" "+count_field+" "+count_neightors;
    send_msg("cmd",friend,strMsg,"step:1");
}

function train2_click(data){
    msg_id+=1;

    var fields_count=s_ui.text_read("tx_time");
    var id=s_ui.text_read("model_id");
    
    var strMsg=python_bin+" /root/train_lr_tcp.py /root/train.txt "+id+" "+fields_count;
    send_msg("cmd",friend,strMsg,"step:2");
    s_ui.text_set("txt_send","");
}

//界面
s_ui.Web_Init("web",10,10,750,300);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.control_dock("progress2","bottom");

s_ui.panel_init("panel_middle",0,0,560,180,"bottom");

s_ui.panel_init("panel_bottom",0,0,300,50,"bottom");


s_ui.button_init("b_train","训练",510,320,200,55,"train_click","");
s_ui.button_init("b_pre", "上一步",100,500,200,30,"next_click","Run_Bat2\\step9");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step_test_knn");


s_ui.panel_init("panel_middle_left",0,0,560,100,"left");

s_ui.panel_add("panel_middle","panel_middle_left","left");


s_ui.label_init("lb_model","模型ID:",160,350);
s_ui.text_init("model_id","1",200,350,160,30);

s_ui.label_init("lb_count","n_neighbors:",160,380);
s_ui.text_init("tx_count","10",200,380,160,30);


s_ui.font_size("lb_model",18);
s_ui.font_size("model_id",18);
s_ui.font_size("lb_count",18);
s_ui.font_size("tx_count",18);


s_ui.progress_init("progress2",10,400,750,30);

s_ui.panel_add("panel_middle","progress2","bottom");

s_ui.control_dock("web","fill");


s_ui.panel_init("panel_middle_left_line2",0,0,300,50,"top");
s_ui.panel_init("panel_middle_left_line3",0,0,300,50,"top");


s_ui.panel_add("panel_middle_left","panel_middle_left_line2","bottom");
s_ui.panel_add("panel_middle_left","panel_middle_left_line3","bottom");


s_ui.panel_add("panel_middle_left_line2","lb_model","right");
s_ui.panel_add("panel_middle_left_line2","model_id","right");

s_ui.panel_add("panel_middle_left_line3","lb_count","right");
s_ui.panel_add("panel_middle_left_line3","tx_count","right");

s_ui.panel_add("panel_bottom","b_train","right");
s_ui.panel_add("panel_bottom","b_pre","right");
s_ui.panel_add("panel_bottom","b_next","right");


s_ui.show_form(800,600);
s_ui.Form_Title("v2 训练模型 KNN");

s_sys.tcp_event();

on_load();

