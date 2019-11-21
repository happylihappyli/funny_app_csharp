

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
            log_msg=s_time.Time_Now()
                +" <span style='color:blue;'>"+obj.from+"</span>"
                +obj.return_cmd+"<br>"+"<pre>"+data+"</pre>"
                +"<br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            switch(obj.return_cmd){
                case "step:1":
                    var strSplit=msg.split(" ");
                    s_ui.text_set("txt_good_sum",strSplit[0]);
                    s_ui.text_set("txt_good",strSplit[0]);
                    static_good_click();
                    break;
                case "step:2":
                    var strSplit=msg.split(" ");
                    s_ui.text_set("txt_bad_sum",strSplit[0]);
                    s_ui.text_set("txt_bad",strSplit[0]);
                    log_msg=s_time.Time_Now()
                        +" <span style='color:blue;'>"+obj.from+"</span>"
                        +"<pre style='color:blue;'>修改抽取的个数！</pre>"
                        +"<br>"+log_msg;
                    s_ui.Web_Content("web",css_head+log_msg);
                    break;
                case "step:11":
                    log_msg=s_time.Time_Now()
                        +" <span style='color:blue;'>"+obj.from+"</span>"
                        +"<pre style='color:red;'>抽取完毕！</pre>"
                        +"<br>"+log_msg;
                    s_ui.Web_Content("web",css_head+log_msg);
                    break;
            }
            break;
        case "status":
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

function read_ini(data){
    var file=disk+"\\Net\\Web\\Data\\memo.ini";
    
    var c1=s_file.Ini_Read(file,"good","compare");
    s_ui.combox_select("combox_good",c1);
    
    var good=s_file.Ini_Read(file,"good","value");
    s_ui.text_set("txt_good",good);
    
    var good_map=s_file.Ini_Read(file,"good","map");
    s_ui.combox_text_set("combox_good_map",good_map);
    
    var c2=s_file.Ini_Read(file,"bad","compare");
    s_ui.combox_select("combox_bad",c2);
    
    var bad=s_file.Ini_Read(file,"bad","value");
    s_ui.text_set("txt_bad",bad);
    
    var bad_map=s_file.Ini_Read(file,"bad","map");
    s_ui.combox_text_set("combox_bad_map",bad_map);
    
}

function static_bad_click(data){
    msg_id+=1;
    var step="1";
    var strMsg="wc -l /root/step8_bad.txt";
    var strType="cmd";
    send_msg(strType,friend,strMsg,"step:"+step);
    s_ui.text_set("txt_send","");
}


function static_good_click(data){
    msg_id+=1;
    
    var step="2";
    var strMsg="wc -l /root/step8_good.txt";
    var strType="cmd";
    
    send_msg(strType,friend,strMsg,"step:"+step);
    
    s_ui.text_set("txt_send","");
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

function merge_click(data){
    msg_id+=1;
    var count=s_ui.text_read("txt_bad");
    var step="11";
    var strMsg="run_js2 /root/happyli/app/merge_good_bad.js 0 /root/step8_good.txt /root/step8_bad.txt /root/test.txt";
    var strType="cmd";
    send_msg(strType,friend,strMsg,"step:"+step);
    s_ui.text_set("txt_send","");
}

//界面
s_ui.Web_Init("web",10,10,750,400);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.label_init("lb_good","好样本个数:",100,420);

s_ui.text_init("txt_good_sum","",200,420,80,30);
s_ui.label_init("lb_good2","抽取个数:",300,420);
s_ui.text_init("txt_good","",400,420,100,30);



s_ui.label_init("lb_bad","坏样本个数:",100,450);

s_ui.text_init("txt_bad_sum","",200,450,80,30);
s_ui.label_init("lb_bad2","抽取个数:",300,450);
s_ui.text_init("txt_bad","",400,450,100,30);

s_ui.button_init("b_get","抽取测试样本",510,420,100,55,"merge_click","");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step_train");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step_lr_test");


s_ui.show_form(800,600);
s_ui.Form_Title("v2 测试 数据抽样");

s_sys.tcp_event();

on_load();

static_bad_click("");


