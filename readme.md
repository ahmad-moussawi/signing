Digital Signing using 3SKey
===========================
This repo contains a toolkit that help you sign files using the 3Skey USB token.

# Limitation
The 3Skey token uses the Java Applet for Web Applications, hence it will not work on browsers that doesn't support Java Applet like Google Chrome.
Currently Tested on FireFox 45, IE 11

# Prerequesites 
- The 3Skey Client software must be installed on the computer that have to sign
- Java 7 and above must be installed (32 bit only) 

# Client Side Installation
- Include the `Web/3skey.js` file into your webpage
   
    ```html
    <script type="text/javascript" src="3skey.js"></script>
    ```
    
- Include the applet tag
    
    ```html
    <div style="visibility:hidden">
        <applet name="pdiApplet" code="com.swift.pdi.applet.TokenApplet" archive="pdiapplet.jar"> </applet>
    </div>
    ```
- Initialize it and Sign

```js
    var token = new Token3Skey(document.pdiApplet);

    window.appletInitialized = function(pSlots){            
        
        if(!pSlots) {
            alert('No USB device found');   
            return;         
        }
                    
        token.setSlot(pSlots);
    }
    
    
    // Sign example
    $('#sign').click(function() {
        var input = $('#input').val();            
        token.sign(input, function(result) {
            $('#result').html(result);
        },function(err, code) {
                alert('Error ' + JSON.stringify({ err: err, code: code }));
        });
    });

    // Sign detached example
    $('#signDetached').click(function() {
        var input = $('#input').val();            
        token.signDetach(input, function(result) {
            $('#result').html(result);
        },function(err, code) {
                alert('Error ' + JSON.stringify({ err: err, code: code }));
            });
    });
```

## Multiple sign example (async signing inside a loop)

```js

var input = '.....',
    separator = '@@',
    digests = input.split(separator),
    signatures = [];
    
for(var i = 0; i < digests.length; i++) {
    (function(i){
        token.signDetach(digests[i], function(signature){
            signatures[i] = signature;
            
            if(i === digests.length - 1) {
                // done
                console.log(signatures.join(separator));                
            }
            
        });
    })(i);
}


```

# JavaScript Api

**sign**
Generate a PKCS7 Attached signature (the file content will be embeded in the signature)

Param  | Type | Description
-------| -----| -----------
content| string| The clear text file
onSuccess| function | a success callback function with the following signature `function(response: string)`
onFailure| function |an error callback function with the following signature `function(err: string, code: string)`
withHeaderAndFooter| bool | Whether to include the "-----BEGIN PKCS7-----" and "-----END PKCS7-----" header and footer

**signDetach**
Generate a PKCS7 Detach signature (the file content will not be embeded in the signature)

Param  | Type | Description
-------| -----| -----------
digest| string| The hash of the file
onSuccess| function | a success callback function with the following signature `function(response: string)`
onFailure| function |an error callback function with the following signature `function(err: string, code: string)`
withHeaderAndFooter| bool | Whether to include the "-----BEGIN PKCS7-----" and "-----END PKCS7-----" header and footer

**logout**
Clear the token current session

Param  | Type | Description
-------| -----| -----------
onSuccess| function | a success callback function with the following signature `function(response: string)`
onFailure| function |an error callback function with the following signature `function(err: string, code: string)`


# Server side setup
- Add the Signing DLL as a reference in your project
- Import the `Signing` namespace
- Import the `Signing.Util` namespace if needed

```cs
var toolkit = new Singing.Toolkit();
var file = new Signing.Util.File();

// Generate a Digest
string digest = toolkit.Disgest(content);

// Generate a PCKS7 Attached Signature
byte[] signature =  toolkit.Sign(new List<byte[]> { signature }, content);

// Convert it to Base64
string signatureB64 = toolkit.ToBase64Format(signature);
```

# Examples 
Check the `Web` and `DemoApp` for more examples

# Copyright
All copyright are reserved for Box & Automation Solutions