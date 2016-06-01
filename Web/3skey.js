/**
 * 3Skey Helper class
 * A wrapper class for the PDI applet JavaScript API
 *
 *  @author Ahmad Moussawi <ahmad.moussawi@treasuryxpress.com>
 * 
 */
; (function (global) {
    function Token3Skey(applet) {
        this.applet = applet;
        this.slot = null;
    }

    Token3Skey.prototype.setSlot = function (slot) {
        this.slot = slot;
        return this;
    }

    /**
     * Generate a PKCS7 Attached signature (the file content will be embedded in the signature)
     * 
     * @param file string the clear text file
     * @param onSuccess function a callback with the following signature `function (response): void`
     * @param onError function a fallback `function (err: string, code: string): void`
     * @param withHeaderAndFooter bool Whether to include the "-----BEGIN PKCS7-----" and "-----END PKCS7-----" header and footer
     * 
     */
    Token3Skey.prototype.sign = function (content, onSuccess, onFailure, withHeaderAndFooter) {
        return this.applet.sign(this.slot, content, {
            onSuccess: function (response) {
                onSuccess(!!withHeaderAndFooter ? attachHeaderAndFooter(response) : response);
            },
            onFailure: onFailure
        });
    }

    /**
     * Generate a PKCS7 Detach signature (the file content will NOT be embedded in the signature)
     * 
     * @param string digest the hash of the file
     * @parma onSuccess function a callback with the following signature `function (response): void`
     * @parma onError function a fallback `function (err: string, code: string): void`
     * @param withHeaderAndFooter bool Whether to include the "-----BEGIN PKCS7-----" and "-----END PKCS7-----" header and footer
     */
    Token3Skey.prototype.signDetach = function (digest, onSuccess, onFailure, withHeaderAndFooter) {
        return this.applet.signDetachSignature(this.slot, digest, {
            onSuccess: function (response) {
                onSuccess(!!withHeaderAndFooter ? attachHeaderAndFooter(response) : response);
            },
            onFailure: onFailure
        });
    }
    
    /**
     * Clear the token current session
     * 
     * @parma onSuccess function a callback with the following signature `function (response): void`
     * @parma onError function a fallback `function (err: string, code: string): void`
     */
    Token3Skey.prototype.logout = function (onSuccess, onFailure) {
        return this.applet.logout(this.slot, {
            onSuccess: onSuccess,
            onFailure: onFailure
        })
    }

    function attachHeaderAndFooter(content) {
        return ["-----BEGIN PKCS7-----", content, "-----END PKCS7-----"].join('\n');
    }

    global.Token3Skey = Token3Skey;
})(window);