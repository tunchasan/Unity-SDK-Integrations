cd "$(dirname "$0")"
echo "splitting jar files…"

jar -xf NativePlugins.jar

echo "extracted files…"

rm -rf NativePlugins.jar

echo "Packing individual jar files…"

echo "Making addressbook.jar…"
jar -cf feature.addressbook.jar com/voxelbusters/nativeplugins/features/addressbook
rm -rf com/voxelbusters/nativeplugins/features/addressbook

echo "Making reachability.jar…"
jar -cf feature.reachability.jar com/voxelbusters/nativeplugins/features/reachability
rm -rf com/voxelbusters/nativeplugins/features/reachability

echo "Making sharing.jar…"
jar -cf feature.sharing.jar com/voxelbusters/nativeplugins/features/sharing
rm -rf com/voxelbusters/nativeplugins/features/sharing

echo "Making billing.jar…"
jar -cf feature.billing.jar com/voxelbusters/nativeplugins/features/billing com/android/vending
rm -rf com/voxelbusters/nativeplugins/features/billing

echo "Making gameservices.jar…"
jar -cf feature.gameservices.jar com/voxelbusters/nativeplugins/features/gameservices
rm -rf com/voxelbusters/nativeplugins/features/gameservices

echo "Making cloudservices.jar…"
jar -cf feature.cloudservices.jar com/voxelbusters/nativeplugins/features/cloudservices
rm -rf com/voxelbusters/nativeplugins/features/cloudservices


echo "Making medialibrary.jar…"
jar -cf feature.medialibrary.jar com/voxelbusters/nativeplugins/features/medialibrary
rm -rf com/voxelbusters/nativeplugins/features/medialibrary

echo "Making notification.jar…"
jar -cf feature.notification.jar com/voxelbusters/nativeplugins/features/notification
rm -rf com/voxelbusters/nativeplugins/features/notification


echo "Making socialnetwork.twitter.jar…"
jar -cf feature.socialnetwork.twitter.jar com/voxelbusters/nativeplugins/features/socialnetwork/twitter
rm -rf com/voxelbusters/nativeplugins/features/socialnetwork/twitter

echo "Making webview.jar…"
jar -cf feature.webview.jar com/voxelbusters/nativeplugins/features/webview
rm -rf com/voxelbusters/nativeplugins/features/webview

echo "Making sdk integration jar files..."
echo "Making sdk.soomla.integration.jar"
jar -cf feature.sdk.soomla.integration.jar com/voxelbusters/nativeplugins/features/external/sdk/soomla
rm -rf com/voxelbusters/nativeplugins/features/external/sdk/soomla


echo "Making external jars…"
jar -cf feature.externallibrary.shortcutbadger.jar com/voxelbusters/nativeplugins/externallibrary/notification/shortcutbadger
rm -rf com/voxelbusters/nativeplugins/externallibrary/notification/shortcutbadger


jar -cf nativeplugins.core.jar com/voxelbusters

rm -rf com
rm -rf META-INF
