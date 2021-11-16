
function submitClick()
{
    let phoneId = document.getElementById("Create_Phone");
    let emailId = document.getElementById("Create_Email");
    document.getElementById("txtSubmitHelp").innerHTML="";
    let lastname = document.getElementById("Create_LastName").className == 'valid';
    let firstname=document.getElementById("Create_FirstName")
    let phone=phoneId.className=='valid';
    let email=emailId.className=='valid';
    if(name&&phone&&email){
        if(phoneId.value!=""|| emailId.value!=""){
            location.replace("./Submit.html")
        }
        else{
            document.getElementById("txtSubmitHelp").innerHTML="Please enter at least one valid contact method";
        }
    }

    else {
        document.getElementById("txtSubmitHelp").innerHTML="Please enter Name, Enquiry, Phone and Email";
    }
}

function validateName(tf, helpText){
    let value=tf.value;
    let regex=/([A-Z]){2,}/i;
    let test=regex.test(value);
    
    if(test){
        tf.className="valid";
        helpText.innerHTML="";
        return true;
    }
    else {
        tf.className = "invalid";
        helpText.innerHTML = "Name must be at Least 2 Letters";
        return false;
    }

}
function validateEmail(tf, helpText){
    let value=tf.value;
    let regex=/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
    let test=regex.test(value);
    if(test){
        tf.className="valid";
        helpText.innerHTML="";
        return true;
    }
    else {
        if(value!=""){
        tf.className = "invalid";
        helpText.innerHTML = "Please Enter Valid Email";
        return false;
        }
        else{
            helpText.innerHTML="";
            tf.className="valid";
            return true
        }
    }

}
function validatePhone(tf, helpText){
    let value=tf.value;
    let regex=/^\d{8,10}$/;;
    let test=regex.test(value);
    if(test){
        tf.className="valid";
        helpText.innerHTML="";
        return true;
    }
        else{{
            tf.className = "invalid";
            helpText.innerHTML = "Enter Valid Phone Number (Numbers Only)";
            return false;
        }
        
            
    }
    
        
    
}

