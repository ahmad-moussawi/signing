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

// Generate a SHA1-256 Digest
string digest = toolkit.Disgest(content);

// Generate a PCKS7 Attached Signature
byte[] signature =  toolkit.Sign(content, new List<byte[]> { signature });

// Convert it to Base64
string signatureB64 = toolkit.ToBase64Format(signature);
```

# Note about certifications
[extracted from this link](https://myonlineusb.wordpress.com/2011/06/19/how-to-convert-certificates-between-pem-der-p7bpkcs7-pfxpkcs12/)

### PEM Format
It is the most common format that Certificate Authorities issue certificates in. It contains the `—–BEGIN CERTIFICATE—–` and `—–END CERTIFICATE—–` statements.

Several PEM certificates and even the Private key can be included in one file, one below the other. But most platforms(eg:- Apache) expects the certificates and Private key to be in separate files.
 - They are Base64 encoded ACII files
 - They have extensions such as .pem, .crt, .cer, .key
 - Apache and similar servers uses PEM format certificates

### DER Format
It is a Binary form of ASCII PEM format certificate. All types of Certificates & Private Keys can be encoded in DER format
 - They are Binary format files
 - They have extensions .cer & .der
 - DER is typically used in Java platform

### P7B/PKCS#7
They contain `—–BEGIN PKCS—–` & `—–END PKCS7—–` statements. It can contain only Certificates & Chain certificates but not the Private key.
 - They are Base64 encoded ASCII files
 - They have extensions .p7b, .p7c
 - Several platforms supports it. eg:- Windows OS, Java Tomcat

### PFX/PKCS#12
They are used for storing the Server certificate, any Intermediate certificates & Private key in one encryptable file.
 - They are Binary format files
 - They have extensions .pfx, .p12
 - Typically used on Windows OS to import and export certificates and Private keys
 
### Difference between `p7s` and `p7m`

the `p7m` contains the signature in addition to the file content, where `p7s` contains only the signature without the file content (detached mode)


# Examples 
Check the `Web`, `Cosign` and `Digest` projects for more examples

# Copyright
All copyright are reserved for Box & Automation Solutions