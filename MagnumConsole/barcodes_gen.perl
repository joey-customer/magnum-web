#!/usr/bin/perl

my $QUANTITY = 5;
my $BATCH = "00001";

my %PROFILES = (
    "MgnmAnastrol"  => $QUANTITY,
    "MgnmBold300" => $QUANTITY,
    "MgnmClen40"  => $QUANTITY,
    "MgnmDbol10"  => $QUANTITY,
    "MgnmDropstanP100"  => $QUANTITY,
    "MgnmMagJack250"  => $QUANTITY,
    "MgnmNandroPlex300"  => $QUANTITY,
    "MgnmOxandro10"  => $QUANTITY,
    "MgnmPrimo100"  => $QUANTITY,
    "MgnmStanol10"  => $QUANTITY,
    "MgnmStanolAQ100"  => $QUANTITY,
    "MgnmTestC300"  => $QUANTITY,
    "MgnmTestE300"  => $QUANTITY,
    "MgnmTestPlex300"  => $QUANTITY,
    "MgnmTestProp100"  => $QUANTITY,
    "MgnmTrenA100"  => $QUANTITY,
    "MgnmTrenE200"  => $QUANTITY,
);

foreach $profile (keys %PROFILES)
{
    $quantity = $PROFILES{$profile};
    execute($profile, $quantity);
}

exit (0);

sub execute
{
    my ($profile, $quantity) = @_;

    my $cmd = <<"END_MESSAGE";
dotnet run -p ../MagnumConsole BarcodeGen \\
--h=https://compute-engine-vm-test.firebaseio.com/ \\
--q=$quantity \\
--b=$BATCH \\
--u=https://development.magnum-pharmacy.com \\
--o='/d/temp' \\
--user=$ENV{'MAGNUM_DB_USERNAME'} \\
--password=$ENV{MAGNUM_DB_PASSWORD} \\
--key=$ENV{MAGNUM_FIREBASE_KEY} \\
--generate=N \\
--profile=$profile
END_MESSAGE

    my $err_msg = system("$cmd");
    my $retcode = ($? >> 8);
}

