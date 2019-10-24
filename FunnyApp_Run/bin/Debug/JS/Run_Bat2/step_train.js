

[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]


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
            switch(obj.return_cmd){
                case "step:1":
                    train2_click("");
                    break;
                case "step:2":
                    var strSplit=msg.split(" ");
                    s_ui.text_set("txt_bad_sum",strSplit[0]);
                    s_ui.text_set("txt_bad",strSplit[0]);
                    log_msg=s_time.Time_Now()
                        +" <span style='color:blue;'>"+obj.from+"</span>"
                        +"<pre style='color:red;'>训练完毕！</pre>"
                        +"<br>"+log_msg;
                    s_ui.Web_Content("web",css_head+log_msg);
                    break;
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


function static_good_click(data){
    msg_id+=1;
    
    var step="2";
    var strMsg="wc -l /root/step8_good.txt";
    var friend="robot1";
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

function train_click(data){
    msg_id+=1;
    var alg=s_ui.combox_text("cb_alg");
    switch(alg){
        case "逻辑回归":
            var file1=s_sys.value_read("file1");
            if (file1=="") file1="E:\\sample1.txt";
            
            var line=s_file.read(file1,1);
            var strSplit=line.split("|");
            var fields_count=strSplit.length;
            s_ui.msg(fields_count);
            var count=0;
            var file=disk+"\\Net\\Web\\Data\\memo.ini";
            for (var i=1;i<=fields_count;i++){
                var value=s_file.Ini_Read(file,"selected","check"+i);
                if (value=="1"){
                    count+=1;
                }
            }
            s_ui.msg(count);
            var id=s_ui.text_read("model_id");
            var step="1";
            var strMsg="/usr/local/bin/python3.7 /root/train_lr_new.py /root/train.txt "+id+" "+count;
            var friend="robot1";
            var strType="cmd";
            send_msg(strType,friend,strMsg,"step:"+step);
            s_ui.text_set("txt_send","");
            break;
    }
}

function train2_click(data){
    msg_id+=1;

    var fields_count=s_ui.text_read("tx_time");
    var id=s_ui.text_read("model_id");
    var step="2";
    var strMsg="/usr/local/bin/python3.7 /root/train_lr.py /root/train.txt "+id+" "+fields_count;
    var friend="robot1";
    var strType="cmd";
    send_msg(strType,friend,strMsg,"step:"+step);
    s_ui.text_set("txt_send","");
}

//界面
s_ui.Web_Init("web",10,10,750,300);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.label_init("lb_alg","算法选择:",100,320);

s_ui.combox_init("cb_alg","逻辑回归",200,320,80,30);
s_ui.combox_add("cb_alg","逻辑回归");

s_ui.label_init("lb_model","模型ID:",100,350);
s_ui.text_init("model_id","1",200,350,80,30);

s_ui.label_init("lb_time","迭代次数:",100,380);
s_ui.text_init("tx_time","600",200,380,80,30);


s_ui.button_init("b_get","训练",510,320,100,55,"train_click","");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step8");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step10");


s_ui.Show_Form(800,600);
s_ui.Form_Title("v2 训练模型");

s_sys.tcp_event();

on_load();

