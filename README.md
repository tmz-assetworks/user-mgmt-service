#**Running ASW Rest APP**
###**● Prerequisites**
- You have a basic understanding of dotnet and rest api apps.
- You have 6.0 runtime, hosting bundle and installed.
- You have kafka service running along with zookeeper, either Azure MSK or as an
independent service (latest stable version) installed on a server.
- You have MSSQL server 2019 installed on a server.
- Docker latest version
- Ngnix should be running (latest stable version)

###**● Configuring and running the application ***
1. Git clone from the repository.
2. cd &lt;base_code_folder&gt;.
3. Configuration
`&quot;AzureAd&quot;: {
   &quot;scopes&quot;: &quot;&lt;scope&gt;&quot;,
   &quot;ClientId&quot;: &quot;&lt;client Id&gt;&quot;,
   &quot;Instance&quot;: &quot;&lt;instance&gt;&quot;,
   &quot;TenantId&quot;: &quot;&lt;TenantId&gt;&quot;,
   &quot;clientSecret&quot;: &quot;&lt;clientSecret&gt;&quot;`

15. Update the docker.yml file as per the env needed for secret manager.
16. cd &lt;base_code_folder&gt;.
17. dotnet restore
18. dotnet build
19. dotnet test
20. dotnet publish --output &lt;path/to/directory&gt; &lt;path/to/project_file&gt;
21. run dll

###**● Running the application using docker:***

Please execute the following instructions from the unix command line
1. Git clone from the repository.
2. cd &lt;base_code_folder&gt;.
3. Configuration
`&quot;AzureAd&quot;: {
   &quot;scopes&quot;: &quot;&lt;scope&gt;&quot;,
   &quot;ClientId&quot;: &quot;&lt;client Id&gt;&quot;,
   &quot;Instance&quot;: &quot;&lt;instance&gt;&quot;,
   &quot;TenantId&quot;: &quot;&lt;TenantId&gt;&quot;,
   &quot;clientSecret&quot;: &quot;&lt;clientSecret&gt;&quot;
   },`

5. cd &lt;base_code_folder&gt;.
6. dotnet restore
7. dotnet build
8. dotnet test
9. docker login &lt;docker_container _registory&gt;
10. username : &lt;username&gt;     Password :&lt;password&gt;
11. docker image build -t &lt;docker_container _registory&gt;/rest_api:M2-release .
12. docker push &lt;docker_container _registory&gt;/rest_api:M2-release
13. run this command on server cmd - &quot;docker run -d --name restapi -p 5003:80
    &lt;docker_container _registory&gt;/rest_api:M2-release&quot;
