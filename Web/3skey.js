/**
 * 3Skey Helper class
 * A wrapper class for the PDI applet JavaScript API
 *
 *  @author Ahmad Moussawi <ahmad.moussawi@treasuryxpress.com>
 * 
 */
; (function(global) {
    function Token3Skey(applet) {
        this.applet = applet;
        this.slot = null;
    }

    Token3Skey.prototype.setSlot = function(slot) {
        this.slot = slot;
        return this;
    }
    
    /**
     * Generate a PKCS7 Attached signature (the file content will be embeded in the signature)
     * 
     * @param string the clear text file
     * @onSuccess function a callback with the following signature `function (response): void`
     * @onError function a fallback `function (err: string, code: string): void`
     * 
     */
    Token3Skey.prototype.sign = function(content, onSuccess, onFailure) {
        return this.applet.sign(this.slot, content, {
            onSuccess: function(response) {
                onSuccess(["-----BEGIN PKCS7-----", response, "-----END PKCS7-----"].join('\n'));
            },
            onFailure: onFailure
        });
    }
    
    /**
     * Generate a PKCS7 Detach signature (the file content will not be embeded in the signature)
     * 
     * @param string digest the hash of the file
     * @onSuccess function a callback with the following signature `function (response): void`
     * @onError function a fallback `function (err: string, code: string): void`
     * 
     */
    Token3Skey.prototype.signDetach = function(digest, onSuccess, onFailure) {
        return this.applet.signDetachSignature(this.slot, digest, {
            onSuccess: function(response) {
                onSuccess(["-----BEGIN PKCS7-----", response, "-----END PKCS7-----"].join('\n'));
            },
            onFailure: onFailure
        });
    }

    Token3Skey.prototype.logout = function(onSuccess, onFailure) {
        return this.applet.logout(this.slot, {
            onSuccess: onSuccess,
            onFailure: onFailure
        })
    }
    global.Token3Skey = Token3Skey;
})(window);